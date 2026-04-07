using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.GetSessionHistory
{
    public class GetSessionHistoryResult
    {
        public bool Success { get; set; }
        public SessionHistoryData? Data { get; set; }
    }

    public class SessionHistoryData
    {
        public IEnumerable<SessionHistoryItemDto> Items { get; set; } = [];
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
