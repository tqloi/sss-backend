using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.CreateAiRoadMap;

namespace SSS.Web.Endpoints.AI.CreateAiRoadMap
{
    public class CreateAiRoadMapEndpoint(ISender sender)
        : Endpoint<CreateAiRoadMapCommand, CreateAiRoadMapResult>
    {
        public override void Configure()
        {
            Post("ai/create-road-map");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CreateAiRoadMapCommand req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);

            await SendAsync(response, cancellation: ct);
        }
    }
}
