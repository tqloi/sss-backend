using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.GetRecentSessions
{
    public class GetRecentSessionsResult
    {
        public bool Success { get; set; }
        public IEnumerable<RecentSessionDto> Data { get; set; } = [];
    }
}
