using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.CreateAiTaskItems;

namespace SSS.Web.Endpoints.AI.CreateAiTaskItems
{
    public class CreateAiTaskItemsEndpoint(ISender sender)
        : Endpoint<CreateAiTaskItemsCommand, CreateAiTaskItemsResult>
    {
        public override void Configure()
        {
            Post("ai/create-task-items");
            AllowAnonymous();
        }
        public override async Task HandleAsync(CreateAiTaskItemsCommand req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
