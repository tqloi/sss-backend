namespace SSS.Application.Features.StudyPlans.TaskItems.Common
{
    public class TaskItemDtos
    {
        public long Id { get; set; }
        public long StudyPlanModuleId { get; set; }
        public bool IsGenerateByAI { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Domain.Enums.TaskStatus? Status { get; set; }
        public int EstimatedDurationSeconds { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class TastItemInput
    {
        public long StudyPlanModuleId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Domain.Enums.TaskStatus? Status { get; set; }
        public int EstimatedDurationSeconds { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}