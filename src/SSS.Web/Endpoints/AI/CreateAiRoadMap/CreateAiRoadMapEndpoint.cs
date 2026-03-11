using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.CreateAiRoadMap;
using System.Security.Claims;

namespace SSS.Web.Endpoints.AI.CreateAiRoadMap
{
    public class CreateAiRoadMapEndpoint(ISender sender)
        : Endpoint<CreateAiRoadMapCommand, CreateAiRoadMapResult>
    {
        public override void Configure()
        {
            Post("api/ai/create-road-map");
            Roles("Admin", "ContentManager");
        }
        public override async Task HandleAsync(CreateAiRoadMapCommand req, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }
            var command = req with { ManagerId = userId };
            var response = await sender.Send(command, ct);

            await SendAsync(response, cancellation: ct);
        }
    }
}
