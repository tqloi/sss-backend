using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanByUser;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.GetStudyPlanByUser
{
    public class GetStudyPlanByUserEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : EndpointWithoutRequest<GetStudyPlanByUserResult>
    {
        public override void Configure()
        {
            Get("/api/study-plans/user");
            Description(d => d.WithTags("StudyPlans"));
            Summary(s => s.Summary = "Get active study plan for authenticated user");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetStudyPlanByUserQuery
            {
                UserId = userId!
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
