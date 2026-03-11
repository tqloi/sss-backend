using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.EndSession
{
    public class EndSessionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public SessionSummaryResponse? Data { get; set; }
    }
}
