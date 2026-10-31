using MediatR;
using nue.protocol.exvs;
using ServerInfinite.Commands.LoadCard.MobileUser;
using ServerInfinite.Commands.LoadCard.PilotData;
using ServerInfinite.Commands.LoadCard.PostProcess;
using ServerInfinite.Constants;
using ServerInfinite.Context.Tracker;
using ServerInfinite.Persistence;
using ServerInfinite.Processor.Tracker;
using Microsoft.EntityFrameworkCore;
using ServerInfinite.Strategy.Team;

namespace ServerInfinite.Handlers.Game;

public record LoadCardQuery(Request Request) : IRequest<Response>;

public class LoadCardQueryHandler : IRequestHandler<LoadCardQuery, Response>
{
    private readonly ILogger<LoadCardQueryHandler> _logger;
    private readonly ServerDbContext _context;
    private readonly ITrackerCatalog _trackerCatalog;

    public LoadCardQueryHandler(ILogger<LoadCardQueryHandler> logger, ServerDbContext context, ITrackerCatalog? trackerCatalog = null)
    {
        _logger = logger;
        _context = context;
        _trackerCatalog = trackerCatalog ?? TrackerCatalog.FromOutput();
    }

    public Task<Response> Handle(LoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;

        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
        };

        var sessionId = request.load_card.SessionId;
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId);

        if (cardProfile == null)
        {
            response.Error = Error.ErrServer;
            return Task.FromResult(response);
        }
        
        var pilotDataGroup = new Response.LoadCard.PilotDataGroup()
        {
            RewardMsIds = null,
            pilot_class_match = new Response.LoadCard.PilotDataGroup.PilotClassMatch(),
            // The new client schema requires practice_info on PilotDataGroup;
            // seed an all-zero default until a Practice subsystem exists server-side.
            PracticeInfo = new Response.LoadCard.PilotDataGroup.PracticeGroup()
        };

        var playerLeaderTeamAppendStrategy = new PlayerLeaderTeamAppendStrategy(_context);
        var playerPartnerTeamAppendStrategy = new PlayerPartnerTeamAppendStrategy(_context);
        
        var pilotDataCommands = new List<IPilotDataCommand>()
        {
            new PilotMobileSuitUsageCommand(_context),
            new TriadMiscInfoCommand(_context),
            new TriadCourseDataCommand(_context),
            new RouteBattleHighScoreCommand(_context),
            new TriadTimeAttackRankRibbonCommand(_context),
            new TriadHighScoreRankRibbonCommand(_context),
            new TriadTargetDestroyRankRibbonCommand(_context),
            new TriadWantedRankRibbonCommand(_context),
            new TrainingDataCommand(_context),
            new SoloClassMatchCommand(_context),
            new TeamClassMatchCommand(_context),
            new LicenseScoreCommand(_context),
            new AppendTeamCommand(playerLeaderTeamAppendStrategy),
            new AppendTeamCommand(playerPartnerTeamAppendStrategy)
        };
        
        pilotDataCommands.ForEach(command => command.Fill(cardProfile, pilotDataGroup));

        var mobileUserGroup = new Response.LoadCard.MobileUserGroup()
        {
            MobileUserId = (uint) cardProfile.Id,
            PaidFlag = LoadCardMobileUserConstants.PaidFlag,
            PlayingStampFlag = LoadCardMobileUserConstants.PlayingStampFlag,
            SupportTicketRemains = LoadCardMobileUserConstants.SupportTicketRemains,
            // New fields required by the realigned client schema. Default placeholders until the
            // corresponding subsystems (pcoin economy, player groups) are wired to real data.
            PcoinTicketNum = 0,
            GroupId = 0,
            MarkerColorId = _context.GeneralSettingDbSet.First(x => x.CardProfile == cardProfile).MarkerColorId,
            online_tag_info = new Response.LoadCard.MobileUserGroup.OnlineTagInfo()
        };
        
        var pilotTrackerContextGenerator = new PilotTrackerContextGenerator(_context);
        var pilotTrackerContext = pilotTrackerContextGenerator.Generate(cardProfile);
        var titleTrackerResolver = new TitleTrackerResolver(_context, cardProfile);
        var msStats = _context.MobileSuitPvPStatisticDbSet.AsNoTracking()
            .Where(x => x.CardId == cardProfile.Id).ToDictionary(x => x.MstMobileSuitId);
        var msUsage = _context.MobileSuitUsageDbSet.AsNoTracking()
            .Where(x => x.CardId == cardProfile.Id).ToDictionary(x => x.MstMobileSuitId, x => x.MsUsedNum);
        var trackers = new StickerTrackerFactory(_trackerCatalog, pilotTrackerContext, titleTrackerResolver, msStats, msUsage);

        var mobileUserCommands = new List<ILoadCardMobileUserCommand>()
        {
            new LoadCustomizeGroupCommand(_context),
            new LoadTriadPartnerCommand(_context),
            new RadarSettingCommand(_context),
            new LoadGamepadCommand(_context),
            new LoadTitleCommand(_context),
            new LoadMessageCommand(_context),
            new OnlinePairCommand(_context),
            new DefaultStickerCommand(_context, trackers),
            new MobileSuitStickerCommand(_context, trackers),
            new QuickTagCommand(_context),
            new LoadQuickStartInfoCommand(_context),
            new LoadStageSaveDataCommand(_context)
        };
        
        mobileUserCommands.ForEach(command => command.Fill(cardProfile, mobileUserGroup));

        // Diagnostic: log the exact command-message payload sent to the real game client so the
        // edit -> store -> send chain can be confirmed against a live card.
        LogCommandMessages(cardProfile.Id, mobileUserGroup);

        var postProcessCommands = new List<ILoadCardPostProcessCommand>()
        {
            new ExTutorialCommand(_context),
            new LatestPlayTimeUpdateCommand(_context)
        };
        
        postProcessCommands.ForEach(command => command.PostProcess(cardProfile, request.load_card));
        
        response.load_card = new Response.LoadCard
        {
            pilot_data_group = pilotDataGroup,
            mobile_user_group = mobileUserGroup,
        };

        return Task.FromResult(response);
    }

    private void LogCommandMessages(int cardId, Response.LoadCard.MobileUserGroup mug)
    {
        string Describe(Response.LoadCard.MobileUserGroup.CommandMessageData d) =>
            d is null
                ? "NULL"
                : $"start='{d.StartMessageText}'({d.StartMessageId}) up='{d.UpMessageText}'({d.UpMessageId}) " +
                  $"down='{d.DownMessageText}'({d.DownMessageId}) left='{d.LeftMessageText}'({d.LeftMessageId}) " +
                  $"right='{d.RightMessageText}'({d.RightMessageId}) end='{d.EndMessageText}'({d.EndMessageId})";

        _logger.LogInformation(
            "LoadCard command messages for CardId={CardId}: MuteFlag={Mute} MessagePosition={Pos} AutoFlag={Auto} | solo[{Solo}] team[{Team}] instantTeam[{Inst}]",
            cardId, mug.MuteFlag, mug.MessagePosition, mug.CommandMessageAutoFlag,
            Describe(mug.SoloCommandMessages), Describe(mug.TeamCommandMessages), Describe(mug.InstantTeamCommandMessages));
    }
}
