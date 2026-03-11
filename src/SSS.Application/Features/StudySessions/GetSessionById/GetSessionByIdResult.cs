using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.GetSessionById
{
    public class GetSessionByIdResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public SessionDetailDto? Data { get; set; }
    }
}
