using MediatR;

namespace SSS.Application.Features.StudyPlans.TaskItems.CreateTask
{
    public class CreateTaskCommand : IRequest<CreateTaskResult>
    {
        public long StudyPlanModuleId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? ExpectedOutput { get; set; }
        public Domain.Enums.TaskStatus? Status { get; set; }
        public int EstimatedDurationSeconds { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
