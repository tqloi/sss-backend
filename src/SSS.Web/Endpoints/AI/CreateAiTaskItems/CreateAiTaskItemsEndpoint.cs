using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using SSS.Application.Features.AI.CreateAiTaskItems;
using SSS.Application.Features.StudyPlans.StudyPlans.CreateStudyPlan;
using System.Security.Claims;

namespace SSS.Web.Endpoints.AI.CreateAiTaskItems
{
    public class CreateAiTaskItemsEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<CreateAiTaskItemsCommand, CreateAiTaskItemsResult>
    {
        public override void Configure()
        {
            Post("/ai/create-task-items");
        }
        public override async Task HandleAsync(CreateAiTaskItemsCommand req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new CreateAiTaskItemsCommand
            {
                UserId = userId!,
                studyPlanModuleId = req.studyPlanModuleId
            };
            var response = await sender.Send(command, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
