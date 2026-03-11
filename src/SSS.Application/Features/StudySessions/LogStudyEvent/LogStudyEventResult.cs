using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.LogStudyEvent
{
    public class LogStudyEventResult
    {
        public bool Success { get; set; }
        public StudyEventDto? Data { get; set; }
    }
}
