using AutoMapper;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Entities.Planning;
using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Features.AI.CreateAiAddBehaviorDb
{
    public class CreateAiAddBehaviorDbMappingProfile : Profile
    {
        public CreateAiAddBehaviorDbMappingProfile()
        {
            CreateMap<StudyPlanModule, BehaviorModuleDto>();
            CreateMap<TaskItem, BehaviorModuleTaskDto>();

            CreateMap<QuizAttempt, BehaviorQuizAttemptDto>()
                .ForMember(d => d.QuizTitle, o => o.MapFrom(s => s.Quiz.Title))
                .ForMember(d => d.AnswersCount, o => o.MapFrom(s => s.Answers.Count));

            CreateMap<StudySession, BehaviorSessionDto>();
            CreateMap<SessionTask, BehaviorSessionTaskDto>()
                .ForMember(d => d.SessionTaskStatus, o => o.MapFrom(s => s.Status))
                .ForMember(d => d.TaskTitle, o => o.MapFrom(s => s.TaskItem.Title))
                .ForMember(d => d.TaskStatus, o => o.MapFrom(s => s.TaskItem.Status))
                .ForMember(d => d.ScheduledDate, o => o.MapFrom(s => s.TaskItem.ScheduledDate))
                .ForMember(d => d.CompletedAt, o => o.MapFrom(s => s.TaskItem.CompletedAt))
                .ForMember(d => d.EstimatedDurationSeconds, o => o.MapFrom(s => s.TaskItem.EstimatedDurationSeconds))
                .ForMember(d => d.StudyPlanModuleId, o => o.MapFrom(s => s.TaskItem.StudyPlanModuleId));
        }
    }

    public sealed record BehaviorModuleDto
    {
        public long Id { get; init; }
        public long StudyPlanId { get; init; }
        public long RoadmapNodeId { get; init; }
        public Domain.Enums.ModuleStatus? Status { get; init; }
        public ICollection<BehaviorModuleTaskDto> Tasks { get; init; } = new List<BehaviorModuleTaskDto>();
    }

    public sealed record BehaviorModuleTaskDto
    {
        public long Id { get; init; }
        public string Title { get; init; } = null!;
        public Domain.Enums.TaskStatus? Status { get; init; }
        public DateTime ScheduledDate { get; init; }
        public DateTime? CompletedAt { get; init; }
        public int EstimatedDurationSeconds { get; init; }
    }

    public sealed record BehaviorQuizAttemptDto
    {
        public long Id { get; init; }
        public long QuizId { get; init; }
        public string? QuizTitle { get; init; }
        public DateTime StartedAt { get; init; }
        public DateTime? SubmittedAt { get; init; }
        public Domain.Enums.QuizAttemptStatus Status { get; init; }
        public decimal? Score { get; init; }
        public int AnswersCount { get; init; }
    }

    public sealed record BehaviorSessionDto
    {
        public string Id { get; init; } = null!;
        public string UserId { get; init; } = null!;
        public DateTime StartAt { get; init; }
        public DateTime? EndAt { get; init; }
        public Domain.Enums.SessionEndedReason? EndedReason { get; init; }
        public int? PlannedDurationSeconds { get; init; }
        public int? ActualDurationSeconds { get; init; }
        public int PauseCount { get; init; }
        public int PauseSeconds { get; init; }
        public int? SelfRating { get; init; }
        public Domain.Enums.LocalTimeBlock? LocalTimeBlock { get; init; }
        public string? Timezone { get; init; }
        public DateTime? CreatedAt { get; init; }
        public DateTime? PausedAt { get; init; }
        public Domain.Enums.SessionStatus Status { get; init; }
        public int? TasksCompletedCount { get; init; }
        public int? TotalTasks { get; init; }
        public long? StudyPlanId { get; init; }
        public long? StudyPlanModuleId { get; init; }
        public List<BehaviorSessionTaskDto> Tasks { get; set; } = new();
    }

    public sealed record BehaviorSessionTaskDto
    {
        public long TaskId { get; init; }
        public string SessionTaskStatus { get; init; } = null!;
        public DateTime? StartTimeUtc { get; init; }
        public DateTime? EndTimeUtc { get; init; }
        public string TaskTitle { get; init; } = null!;
        public Domain.Enums.TaskStatus? TaskStatus { get; init; }
        public DateTime ScheduledDate { get; init; }
        public DateTime? CompletedAt { get; init; }
        public int EstimatedDurationSeconds { get; init; }
        public long StudyPlanModuleId { get; init; }
    }
}
