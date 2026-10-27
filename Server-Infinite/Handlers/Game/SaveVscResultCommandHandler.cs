using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerInfinite.Commands.Common;
using ServerInfinite.Commands.SaveBattle;
using ServerInfinite.Commands.SaveBattle.Common;
using ServerInfinite.Commands.SaveBattle.Triad;
using ServerInfinite.Commands.SaveBattle.Triad.Ranking;
using ServerInfinite.Context.Battle;
using ServerInfinite.Mapper.Context;
using ServerInfinite.Models.Cards;
using ServerInfinite.Models.Config;
using ServerInfinite.Persistence;
using WebUIInfinite.Shared.Dto.Enum;

namespace ServerInfinite.Handlers.Game;

public record SaveVscResultCommand(Request Request) : IRequest<Response>;

public class SaveVscResultCommandHandler : IRequestHandler<SaveVscResultCommand, Response>
{
    private readonly ILogger<SaveVscResultCommandHandler> _logger;
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;

    public SaveVscResultCommandHandler(ILogger<SaveVscResultCommandHandler> logger, ServerDbContext context, IOptions<CardServerConfig> options)
    {
        _logger = logger;
        _context = context;
        _config = options.Value;
    }
    
    public Task<Response> Handle(SaveVscResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vsc_result.SessionId;
        var cardId = request.Request.save_vsc_result.PilotId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);

        if (cardProfile == null)
        {
            return Task.FromResult(new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_vsc_result = new Response.SaveVscResult()
            });
        }

        var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
        var currentYear = (uint) currentTime.Year;
        var currentMonth = (uint) currentTime.Month;
        
        var playResultGroup = request.Request.save_vsc_result.Result;
        var battleResultContext = playResultGroup.ToBattleResultContext();

        if (playResultGroup.ShouldSerializeSkillPointMobileSuitId())
            battleResultContext.MobileSuitMasteryDomain.MasteryMobileSuitId = playResultGroup.SkillPointMobileSuitId;

        battleResultContext.CommonDomain.BattleMode = BattleModeConstant.Triad;

        if (!playResultGroup.ShouldSerializeCourseId())
        {
            // vsac29 route battle: no Triad course fields (50+) are sent.
            return Task.FromResult(SaveRouteBattle(request, cardProfile, playResultGroup, battleResultContext));
        }
        
        var saveBattleDataCommands = new List<ISaveBattleDataCommand>()
        {
            // Common Commands
            new SaveGpCommand(_context),
            new SavePlayerLevelCommand(_context),
            new SaveNaviCommand(_context),
            new SaveMobileSuitMasteryCommand(_context),
            new SaveTeamCommand(_context),
            // Triad Mode Exclusive Commands
            new SaveTriadPartnerCommand(_context),
            new SaveTriadMiscInfoCommand(_context),
            new SaveTriadCourseScoreCommand(_context),
            new AddReleaseTriadCourseCommand(_context),
            // Triad Ranking Commands
            new SaveTriadRankTargetCommand(_context, currentYear, currentMonth),
            new SaveTriadRankWantedCommand(_context, currentYear, currentMonth),
            new SaveTriadRankHighScoreCommand(_context, _config, currentYear, currentMonth),
            new SaveTriadRankClearTimeCommand(_context, _config, currentYear, currentMonth)
        };
        
        saveBattleDataCommands.ForEach(command => command.Save(cardProfile, battleResultContext));

        new SaveBadgeCommand(_context, _config, _logger).Save(cardProfile, playResultGroup.ObtainedBadgePoint);
        new SaveQuickStartSettingCommand(_context).Save(cardProfile, playResultGroup.QuickStartInfo);
        
        _context.SaveChanges();
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsc_result = new Response.SaveVscResult()
        });
    }

    /// <summary>
    /// Route battle result (vsac29). Saves GP / player level / navi / mastery,
    /// adds obtained_badge_point (tag 25) to BadgeExp and saves the quick start setting.
    /// </summary>
    private Response SaveRouteBattle(SaveVscResultCommand request, CardProfile cardProfile,
        Request.SaveVscResult.PlayResultGroup result, BattleResultContext battleResultContext)
    {
        _logger.LogInformation("SaveVscResult(route) CardId={CardId} stage={Stage} phase={Phase} type={Type} difficulty={Difficulty} win={Win} gauge={Before}->{After} level={LvBefore}->{LvAfter} exp+{Exp} badge_point={Badge} one_on_one={OneOnOne} options=[{Options}]",
            cardProfile.Id, result.StageNum, result.PhaseNum, result.BattleType, result.Difficulty, result.WinFlag,
            result.BeforeGauge, result.AfterGauge, result.LevelId, result.PlayerLevelId, result.LevelExp,
            result.ObtainedBadgePoint, result.OneOnOneFlag, string.Join(",", result.BattleOptions ?? Array.Empty<uint>()));

        var routeBattleCommands = new List<ISaveBattleDataCommand>()
        {
            new SaveGpCommand(_context),
            new SavePlayerLevelCommand(_context),
            new SaveNaviCommand(_context),
            new SaveMobileSuitMasteryCommand(_context)
        };

        routeBattleCommands.ForEach(command => command.Save(cardProfile, battleResultContext));

        new SaveBadgeCommand(_context, _config, _logger).Save(cardProfile, result.ObtainedBadgePoint);
        new SaveQuickStartSettingCommand(_context).Save(cardProfile, result.QuickStartInfo);

        _context.SaveChanges();

        return new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsc_result = new Response.SaveVscResult()
        };
    }
}
