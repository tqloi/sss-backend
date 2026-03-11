using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.CreateAiAddVecDb;

namespace SSS.Web.Endpoints.AI.CreateAiAddVecDb
{
    public class CreateAiAddVecDbEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<CreateAiAddVecDbCommand, CreateAiAddVecDbResponse>
    {
        public override void Configure()
        {
            Post("ai/add-vec-db");

        }
        public override async Task HandleAsync(CreateAiAddVecDbCommand req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var command = new CreateAiAddVecDbCommand
            {
                UserId = userId!.Value,
                StudyPlanId = req.StudyPlanId
            };
            var response = await sender.Send(command, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
