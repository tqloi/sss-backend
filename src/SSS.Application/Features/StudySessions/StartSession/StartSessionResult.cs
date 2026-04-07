using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.StartSession
{
    public class StartSessionResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public StartSessionResponse? Data { get; set; }
    }
}
