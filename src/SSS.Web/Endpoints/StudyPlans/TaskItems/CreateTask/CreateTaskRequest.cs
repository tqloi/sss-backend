namespace SSS.Web.Endpoints.StudyPlans.TaskItems.CreateTask
{
    public class CreateTaskRequest
    {
        public long StudyPlanModuleId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public Domain.Enums.TaskStatus? Status { get; set; }
        public int EstimatedDurationSeconds { get; set; }
        public DateTime ScheduledDate { get; set; }
    }
}
