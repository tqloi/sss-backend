using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.GetActiveSession
{
    public class GetActiveSessionResult
    {
        public bool Success { get; set; }
        public ActiveSessionDto? Data { get; set; }
    }
}
