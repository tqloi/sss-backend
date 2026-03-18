using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.CreateAiAddBehaviorDb;
using SSS.Application.Features.AI.CreateAiAddVecDb;

namespace SSS.Web.Endpoints.AI.CreateAiAddBehaviorDb
{
    public class CreateAiAddBehaviorDbEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<CreateAiAddBehaviorDbCommand, CreateAiAddBehaviorDbResult>
    {
        public override void Configure()
        {
            Post("/ai/add-behavior-db");

        }
        public override async Task HandleAsync(CreateAiAddBehaviorDbCommand req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var command = new CreateAiAddBehaviorDbCommand
            {
                UserId = userId!.Value,
                StudyPlanId = req.StudyPlanId
            };
            var response = await sender.Send(command, ct);
            await SendAsync(response, cancellation: ct);
        }

    }
}
