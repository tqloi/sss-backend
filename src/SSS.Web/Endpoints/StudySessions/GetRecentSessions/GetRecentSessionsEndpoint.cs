using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.GetRecentSessions;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.GetRecentSessions
{
    public class GetRecentSessionsRequest
    {
        [QueryParam] public int Limit { get; set; } = 5;
    }

    public class GetRecentSessionsEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetRecentSessionsRequest, GetRecentSessionsResult>
    {
        public override void Configure()
        {
            Get("/api/study-sessions/recent");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Get recent completed sessions for dashboard widget");
        }

        public override async Task HandleAsync(GetRecentSessionsRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetRecentSessionsQuery
            {
                UserId = userId!,
                Limit = req.Limit
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
