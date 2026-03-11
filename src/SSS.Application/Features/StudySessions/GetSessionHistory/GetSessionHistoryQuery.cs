using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.GetSessionHistory
{
    public class GetSessionHistoryQuery : IRequest<GetSessionHistoryResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "date";
        public string SortOrder { get; set; } = "desc";
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Status { get; set; }
    }
}
