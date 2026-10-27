using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerInfinite.Dtos.Request;
using ServerInfinite.Handlers.UI.Card;
using WebUIInfinite.Shared.Dto.Request;
using WebUIInfinite.Shared.Dto.Response;
using WebUIInfinite.Shared.Dto.Statistics;

namespace ServerInfinite.Controllers.UI;

[ApiController]
[Route("ui/card")]
public class CardController : BaseController<CardController>
{
    private readonly IMediator _mediator;

    public CardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getAll")]
    [Produces("application/json")]
    public async Task<ActionResult<List<BareboneCardProfile>>> GetAll([FromQuery] GetAllBareboneCardRequest request)
    {
        var response = await _mediator.Send(new GetAllBareboneCardCommand(request));
        return response.Data.ToList();
    }
    
    [HttpGet("getBy/{accessCode}")]
    [Produces("application/json")]
    public async Task<ActionResult<BareboneCardProfile>> GetByAccessCode(String accessCode)
    {
        var response = await _mediator.Send(new GetByAccessCodeCommand(accessCode));
        return response;
    }

    [HttpPost("authorize")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> Authorize([FromBody] CardAuthorizationRequest request)
    {
        var response = await _mediator.Send(new PostCardAuthorizationCommand(request));
        return response;
    }
    
    [HttpGet("getBasicDisplayProfile/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicDisplayProfile>> GetBasicDisplayProfile(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetBasicDisplayProfileCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("updateBasicProfile")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpdateBasicProfile([FromBody] UpdateBasicProfileRequest request)
    {
        var response = await _mediator.Send(new UpdateBasicProfileCommand(request));
        return response;
    }

    [HttpGet("statistics/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<PlayerStatisticsDto>> GetStatistics(String accessCode, String chipId)
    {
        var response = await _mediator.Send(new GetPlayerStatisticsCommand(accessCode, chipId));
        return response;
    }
}