using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.CreateRoadMap;

namespace SSS.Web.Endpoints.AI.CreateRoadMap
{
    public class CreateRoadMapEndpoint(ISender sender)
        : Endpoint<CreateRoadMapCommand, CreateRoadMapResult>
    {
        public override void Configure()
        {
            Post("ai/create-road-map");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CreateRoadMapCommand req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
