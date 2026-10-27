using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerInfinite.Commands.SaveBattle.Common;
using ServerInfinite.Models.Config;
using ServerInfinite.Persistence;

namespace ServerInfinite.Handlers.Game.Online;

public record SaveVsmOnResultCommand(Request Request) : IRequest<Response>;

/// <summary>
/// Online battle result. Only the badge part is stored (the rest was never implemented):
/// result.badge_id (tag 19) -> PlayerLevel.BadgeId, result.obtained_badge_point (tag 69) -> BadgeExp +=.
/// </summary>
public class SaveVsmOnResultCommandHandler : IRequestHandler<SaveVsmOnResultCommand, Response>
{
    private readonly ILogger<SaveVsmOnResultCommandHandler> _logger;
    private readonly ServerDbContext _context;
    private readonly CardServerConfig _config;
    
    public SaveVsmOnResultCommandHandler(ILogger<SaveVsmOnResultCommandHandler> logger, ServerDbContext context, IOptions<CardServerConfig> options)
    {
        _logger = logger;
        _context = context;
        _config = options.Value;
    }
    
    public Task<Response> Handle(SaveVsmOnResultCommand request, CancellationToken cancellationToken)
    {
        var save = request.Request.save_vsm_on_result;
        if (save != null && save.Result != null && !string.IsNullOrEmpty(save.SessionId))
        {
            var cardProfile = _context.CardProfiles
                .FirstOrDefault(x => x.SessionId == save.SessionId && x.Id == save.PilotId);
            if (cardProfile != null)
            {
                _logger.LogInformation("SaveVsmOnResult CardId={CardId} badge_id={BadgeId} badge_point={Point}",
                    cardProfile.Id, save.Result.BadgeId, save.Result.ObtainedBadgePoint);
                // exp delta first, then the reported badge (a badge change may reset exp, see SaveBadgeCommand).
                var badge = new SaveBadgeCommand(_context, _config, _logger);
                badge.Save(cardProfile, save.Result.ObtainedBadgePoint);
                badge.ApplyReportedBadge(cardProfile, save.Result.BadgeId, "SaveVsmOnResult");
                _context.SaveChanges();
            }
        }

        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsm_on_result = new Response.SaveVsmOnResult()
        });
    }
}
