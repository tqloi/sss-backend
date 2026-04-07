using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.GetSessionStatistics
{
    public class GetSessionStatisticsResult
    {
        public bool Success { get; set; }
        public SessionStatisticsDto? Data { get; set; }
    }
}
