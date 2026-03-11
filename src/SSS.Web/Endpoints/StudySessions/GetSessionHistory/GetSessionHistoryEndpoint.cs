using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.GetSessionHistory;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.GetSessionHistory
{
    public class GetSessionHistoryRequest
    {
        [QueryParam] public int PageNumber { get; set; } = 1;
        [QueryParam] public int PageSize { get; set; } = 10;
        [QueryParam] public string SortBy { get; set; } = "date";
        [QueryParam] public string SortOrder { get; set; } = "desc";
        [QueryParam] public string? StartDate { get; set; }
        [QueryParam] public string? EndDate { get; set; }
        [QueryParam] public string? Status { get; set; }
    }

    public class GetSessionHistoryEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetSessionHistoryRequest, GetSessionHistoryResult>
    {
        public override void Configure()
        {
            Get("/api/study-sessions/history");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Get paginated session history with filters");
        }

        public override async Task HandleAsync(GetSessionHistoryRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetSessionHistoryQuery
            {
                UserId = userId!,
                PageNumber = req.PageNumber,
                PageSize = req.PageSize,
                SortBy = req.SortBy,
                SortOrder = req.SortOrder,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                Status = req.Status
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
