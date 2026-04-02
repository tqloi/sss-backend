using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.LearningSubject.GetByContentManager;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Content.LearningSubject.GetByContentManager
{
    public sealed class GetSubjectByContentManagerEndpoint(ISender sender)
        : EndpointWithoutRequest<GetSubjectByContentManagerResult>
    {
        public override void Configure()
        {
            Get("/api/learning-subjects/manager");
            Summary(s => s.Summary = "Get subjects assigned to the logged-in content manager");
            Description(d => d.WithTags("LearningSubjects"));
            Roles("ContentManager");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(managerId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var query = new GetSubjectByContentManagerQuery
            {
                ManagerId = managerId
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
