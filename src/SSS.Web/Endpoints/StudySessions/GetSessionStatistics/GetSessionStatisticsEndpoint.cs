using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.GetSessionStatistics;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.GetSessionStatistics
{
    public class GetSessionStatisticsRequest
    {
        [QueryParam] public string? Period { get; set; }
    }

    public class GetSessionStatisticsEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetSessionStatisticsRequest, GetSessionStatisticsResult>
    {
        public override void Configure()
        {
            Get("/api/study-sessions/statistics");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Get session statistics for the dashboard");
        }

        public override async Task HandleAsync(GetSessionStatisticsRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetSessionStatisticsQuery
            {
                UserId = userId!,
                Period = req.Period
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
