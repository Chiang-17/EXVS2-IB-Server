using MediatR;
using WebUIInfinite.Shared.Dto.Request;
using WebUIInfinite.Shared.Dto.Response;

namespace ServerInfinite.Handlers.UI.Player;

public record UpdatePlayerLevelRoundCommand(BasicCardRequest Request) : IRequest<BasicResponse>;

public class UpdatePlayerLevelRoundCommandHandler : IRequestHandler<UpdatePlayerLevelRoundCommand, BasicResponse>
{
    // Retain the old endpoint for cached clients, but never reset player data.
    public Task<BasicResponse> Handle(UpdatePlayerLevelRoundCommand request, CancellationToken cancellationToken)
        => Task.FromResult(new BasicResponse { Success = false });
}
