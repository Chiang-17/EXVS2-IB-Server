using MediatR;
using Microsoft.EntityFrameworkCore;
using nue.protocol.exvs;
using ServerInfinite.Commands.PreLoadCard;
using ServerInfinite.Commands.PreLoadCard.LoadPlayer;
using ServerInfinite.Commands.PreLoadCard.MobileUserGroup;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;

namespace ServerInfinite.Handlers.Game;

public record PreLoadCardQuery(Request Request) : IRequest<Response>;

public class PreLoadCardQueryHandler : IRequestHandler<PreLoadCardQuery, Response>
{
    private readonly ILogger<PreLoadCardQueryHandler> _logger;
    private readonly ServerDbContext _context;

    public PreLoadCardQueryHandler(ILogger<PreLoadCardQueryHandler> logger, ServerDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Response> Handle(PreLoadCardQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success
        };

        var preLoadCardRequest = request.pre_load_card;
        
        var cardProfile = await _context.CardProfiles
            .FirstOrDefaultAsync(x => x.AccessCode == preLoadCardRequest.AccessCode && x.ChipId == preLoadCardRequest.ChipId, cancellationToken);
        
        var sessionId = preLoadCardRequest.AccessCode + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        if (cardProfile != null)
        {
            return await ReadAndReturn(preLoadCardRequest, cardProfile, sessionId, response);
        }
        
        _logger.LogInformation("Card not exist for ChipId = {ChipId}, Now creating...", preLoadCardRequest.ChipId);

        var newCardProfile = new CardProfile()
        {
            AccessCode = preLoadCardRequest.AccessCode,
            ChipId = preLoadCardRequest.ChipId,
            SessionId = sessionId,
            UserName = "EXVS2-" + GetRandomAlphaNumeric(6),
            IsNewCard = true,
            DistinctTeamFormationToken = Guid.NewGuid().ToString("n").Substring(0, 16),
            LastPlayedAt = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds()
        };

        // Tracker slots default to None (0); player configures them in WebUI / game.
        newCardProfile.DefaultStickerProfile.Tracker1 = 0;
        newCardProfile.DefaultStickerProfile.Tracker2 = 0;
        newCardProfile.DefaultStickerProfile.Tracker3 = 0;
        
        _context.CardProfiles.Add(newCardProfile);
        await _context.SaveChangesAsync(cancellationToken);
        
        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            IsNewCard = true
        };
        
        return response;
    }

    private async Task<Response> ReadAndReturn(Request.PreLoadCard preLoadCardRequest, CardProfile cardProfile,
        string sessionId, Response response)
    {
        _logger.LogInformation("Card exists for ChipId = {ChipId}, Now reading from Database",
            preLoadCardRequest.ChipId);
        
        cardProfile.SessionId = sessionId;
        await _context.SaveChangesAsync();
        
        if (cardProfile.IsNewCard)
        {
            _logger.LogInformation("ChipId = {} is still a new card, will go to RegisterCard", preLoadCardRequest.ChipId);
            response.pre_load_card = new Response.PreLoadCard
            {
                SessionId = sessionId,
                AcidResponse = null,
                AcidError = AcidError.AcidSuccess,
                IsNewCard = true
            };
            return response;
        }
        
        var gamepadSetting = await _context.GamepadSettingDbSet
            .FirstAsync(x => x.CardProfile == cardProfile);

        var mobileUserGroup = new Response.PreLoadCard.MobileUserGroup()
        {
            BadgeId = 0,
            BadgeExp = 0,
            AuthFlag = true,
            GamePadStyle = gamepadSetting.GamePadStyle,
            customize_group = new Response.PreLoadCard.MobileUserGroup.CustomizeGroup()
            {
                NaviBoostRemains = 9999,
                GuestNavAnnivFlag = false,
                // `quick_start_flag` (tag 13) filled by QuickStartFlagCommand from persisted data.
                QuickStartFlag = false,
                // Client `command_disp_config` (CustomizeGroup tag 14). Command Display enum.
                CommandDispConfig = gamepadSetting.CommandDispConfig,
                // Server-only legacy fields (tags 51, 52) retained until WebUI is migrated.
                GpBoostRemains = 9999,
                TagSkillPointBoostFlag = true,
            }
        };
        
        var mobileUserGroupCommands = new List<IPreLoadMobileUserGroupCommand>()
        {
            new PreLoadMobileUserBasicInformationCommand(_context),
            new DisplaySettingCommand(_context),
            new PreLoadTitleCommand(_context),
            new PreLoadTriadPartnerCommand(_context),
            new NaviSettingCommand(_context),
            new BoostSettingCommand(_context),
            new NaviCommand(_context),
            new MobileSuitCommand(_context),
            new ChallengeMissionCommand(_context),
            new QuickStartFlagCommand(_context)
        };
        
        mobileUserGroupCommands.ForEach(command => command.Fill(cardProfile, mobileUserGroup));

        // LoadPlayer tags 40-43 were rebuilt to match the new client schema
        // (rate_solo / rate_team / grade_1_solo / grade_1_team). The two grade flags
        // are required, so seed safe defaults; the per-class fillers below still
        // populate rate_solo / rate_team.
        var loadPlayer = new Response.PreLoadCard.LoadPlayer
        {
            Grade1Solo = false,
            Grade1Team = false
        };

        var preLoadPlayerCommands = new List<IPreLoadPlayerCommand>()
        {
            new PreLoadPlayerBasicInformationCommand(_context),
            new PlayerLevelCommand(_context),
            new SoloClassInformationCommand(_context),
            new TeamClassInformationCommand(_context),
            new WinLossDataCommand(_context)
        };
        
        preLoadPlayerCommands.ForEach(command => command.Fill(cardProfile, loadPlayer));
        
        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            AmId = (uint) cardProfile.Id,
            IsNewCard = false,
            User = mobileUserGroup,
            load_player = loadPlayer
        };
        
        var preLoadCardCommands = new List<IPreLoadCardCommand>()
        {
            new PrivateRoomCommand(_context)
        };
        
        preLoadCardCommands.ForEach(command => command.Fill(cardProfile, response.pre_load_card));
        
        return response;
    }

    string GetRandomAlphaNumeric(int length)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var list = Enumerable.Repeat(0, length).Select(x=>chars[random.Next(chars.Length)]);
        return string.Join("", list);
    }
}