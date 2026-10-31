using nue.protocol.exvs;
using ServerInfinite.Models.Cards;
using ServerInfinite.Persistence;
using MediatR;
using Microsoft.Extensions.Options;
using ServerInfinite.Models.Config;
using ServerInfinite.Commands.SaveBattle.Common;

namespace ServerInfinite.Handlers.Game;

public record SaveUserPlayResearchDataCommand(Request Request) : IRequest<Response>;

/// <summary>
/// vsac29 sends the player's current badge_id (tag 6) after every play, including CPU / route battle.
/// Badge up/down is decided by the game; the server just stores what it reports.
/// </summary>
public class SaveUserPlayResearchDataCommandHandler : IRequestHandler<SaveUserPlayResearchDataCommand, Response>
{
    private readonly ILogger<SaveUserPlayResearchDataCommandHandler> _logger;

    private readonly ServerDbContext _context;

    private readonly CardServerConfig _config;

    public SaveUserPlayResearchDataCommandHandler(ILogger<SaveUserPlayResearchDataCommandHandler> logger, ServerDbContext context, IOptions<CardServerConfig> options)
    {
        _logger = logger;
        _context = context;
        _config = options.Value;
    }

    public Task<Response> Handle(SaveUserPlayResearchDataCommand request, CancellationToken cancellationToken)
    {
        Request.SaveUserPlayResearchData data = request.Request.save_user_play_research_data;
        bool enabled = _config?.GameConfigurations?.RouteBattleConfigurations?.BadgeIdFromResearchData ?? true;
        if (enabled && data != null && !string.IsNullOrEmpty(data.SessionId))
        {
            string sessionId = data.SessionId;
            long pilotId = data.PilotId;
                var cardProfile = _context.CardProfiles.FirstOrDefault(x => x.SessionId == sessionId && (long)x.Id == pilotId);
            if (cardProfile != null)
            {
                _logger.LogInformation("SaveUserPlayResearchData CardId={CardId} mode={Mode} player_level={Level} badge_id={BadgeId}", cardProfile.Id, data.Mode, data.PlayerLevelId, data.BadgeId);
                new SaveBadgeCommand(_context, _config, _logger).ApplyReportedBadge(cardProfile, data.BadgeId, "SaveUserPlayResearchData");
                _context.SaveChanges();
            }
            else
            {
                _logger.LogWarning("SaveUserPlayResearchData: card not found. PilotId={PilotId}", data.PilotId);
            }
        }
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_user_play_research_data = new Response.SaveUserPlayResearchData()
        });
    }
}
