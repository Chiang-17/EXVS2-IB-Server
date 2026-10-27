using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerInfinite.Handlers.UI.Player;
using ServerInfinite.Persistence;
using WebUIInfinite.Shared.Dto.Player;
using WebUIInfinite.Shared.Dto.Request;
using WebUIInfinite.Shared.Dto.Response;

namespace ServerInfinite.Controllers.UI;

[ApiController]
[Route("ui/player-level")]
public class PlayerLevelController : BaseController<PlayerLevelController>
{
    private readonly IMediator _mediator;

    public PlayerLevelController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("getPlayerLevelProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<PlayerLevelProfile>> GetPlayerLevelProfile(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetPlayerLevelCommand(accessCode, chipId));
        return response;
    }
    
    /// <summary>Route battle badge of a card (also included in getPlayerLevelProfile).</summary>
    [HttpGet("getBadge/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public ActionResult<BadgeInfo> GetBadge(string accessCode, string chipId, [FromServices] ServerDbContext context)
    {
        var cardProfile = context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == accessCode && x.ChipId == chipId);
        if (cardProfile == null)
        {
            return NotFound();
        }

        var cardId = cardProfile.Id;
        var playerLevel = context.PlayerLevelDbSet.FirstOrDefault(x => x.CardId == cardId);
        return new BadgeInfo
        {
            BadgeId = playerLevel?.BadgeId ?? 0u,
            BadgeExp = playerLevel?.BadgeExp ?? 0
        };
    }

    public class BadgeInfo
    {
        public uint BadgeId { get; set; }
        public int BadgeExp { get; set; }
    }
    
    [HttpPost("updatePlayerLevelRound")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdatePlayerLevelRound([FromBody] BasicCardRequest request)
    {
        var response = await _mediator.Send(new UpdatePlayerLevelRoundCommand(request));
        return response;
    }
}