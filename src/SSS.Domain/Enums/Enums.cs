namespace SSS.Domain.Enums
{
    public enum RoadmapStatus { Draft, Active, Disabled, Archived }
    public enum StudyPlanStatus { Draft, Active, Archived, GeneratingTasks, Ready, Failed }
    public enum StudyPlanStrategy { Balanced, Speed, Depth }
    public enum ModuleStatus { Locked, Active, Completed, Skipped }
    public enum TaskStatus { Pending, Scheduled, InProgress, Completed, Skipped, Archived }

    // Sessions
    public enum SessionStatus { NotStarted, InProgress, Paused, Completed, Cancelled }
    public enum SessionEndedReason { Completed, TimedOut, Cancelled, SystemTerminated }
    public enum LocalTimeBlock { Morning, Afternoon, Evening, Night }
    public enum SessionEventType
    {
        SessionStart,
        SessionPause,
        SessionResume,
        SessionEnd,
        TaskStart,
        TaskComplete,
        TaskSkip,
        FeedbackSubmitted
    }

    // Survey
    public enum SurveyStatus { Draft, Published, Archived }
    public enum SurveyTriggerReason { Initial, Resurvey, Manual }
    public enum SurveyQuestionType { SingleChoice, MultipleChoice, Scale, ShortAnswer, FreeText }
    public enum TargetStatus
    {
        active,
        archived,
        completed
    }

    public enum DisciplineType
    {
        OnTime,
        LateButDone,
        DropMidway
    }

    // Quiz
    public enum QuizQuestionType { SingleChoice, MultipleChoice, ShortAnswer }
    public enum QuizAttemptStatus { InProgress, Passed, Failed }

    // Roadmap / Content
    public enum NodeDifficulty { Beginner, Intermediate, Advanced }
    public enum ContentType { Video, Article, Book, Course, Exercise, Quiz, Project, }
    public enum EdgeType { Prerequisite, Recommended, Next }

    // Notification
    public enum NotificationType { System, Reminder, Achievement, Resurvey, AiRecommendation }
    public enum NotificationRelatedType { None, Task, Module, Plan, Node, Session, Roadmap }
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    // AI (if you keep AI messages in SQL)
    public enum AiMessageRole { User, Admin, System }

    public enum AiRecommendationType
    {
        StudyTip = 1,
        Reminder = 2,
        Suggestion = 3
    }

    public enum RelatedEntityType
    {
        Lesson = 1,
        Course = 2,
        StudySession = 3,
        Module = 4,
        Task = 5
    }

    /* =======================
     * STUDY / TRACKING
     * ======================= */

    public enum StudyEventType
    {
        View = 1,
        Click = 2,
        Start = 3,
        Submit = 4,
        Complete = 5
    }

    public enum StudyEventCategory
    {
        Learning = 1,
        Assessment = 2,
        Navigation = 3,
        System = 4
    }

    public enum ContentMode
    {
        Video = 1,
        Text = 2,
        Quiz = 3,
        Practice = 4
    }

    public enum SubscriptionType
    {
        Free = 1,
        Premium = 2,
        Pro = 3
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Success = 1,
        Failed = 2,
        Canceled = 3
    }
}
