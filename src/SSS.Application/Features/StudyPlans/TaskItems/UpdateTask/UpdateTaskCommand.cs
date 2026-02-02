using MediatR;

namespace SSS.Application.Features.StudyPlans.TaskItems.UpdateTask
{
    public class UpdateTaskCommand : IRequest<UpdateTaskResult>
    {
        public long TaskId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Domain.Enums.TaskStatus? Status { get; set; }
        public int? EstimatedDurationSeconds { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
