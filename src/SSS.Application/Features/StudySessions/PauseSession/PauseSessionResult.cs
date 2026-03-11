using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.PauseSession
{
    public class PauseSessionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public PauseSessionResponse? Data { get; set; }
    }
}
