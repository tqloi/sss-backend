using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.ResumeSession
{
    public class ResumeSessionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public ResumeSessionResponse? Data { get; set; }
    }
}
