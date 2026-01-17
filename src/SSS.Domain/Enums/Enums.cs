namespace SSS.Domain.Enums
{
    public enum StudyPlanStatus { Draft, Active, Archived }
    public enum StudyPlanStrategy { Balanced, Speed, Depth }
    public enum ModuleStatus { Locked, Active, Completed, Skipped }
    public enum TaskStatus { Pending, Scheduled, InProgress, Completed, Skipped, Archived }

    // Sessions
    public enum SessionEndedReason { Completed, Stopped, Timeout, Abandoned, Error }
    public enum LocalTimeBlock { Morning, Afternoon, Evening, Night }

    // Survey
    public enum SurveyStatus { Draft, Published, Archived }
    public enum SurveyTriggerReason { Initial, Resurvey, Manual }
    public enum SurveyQuestionType { SingleChoice, MultipleChoice, Scale, ShortAnswer, FreeText }

    // Quiz
    public enum QuizQuestionType { SingleChoice, MultipleChoice, Scale, ShortAnswer }
    public enum QuizAttemptStatus { InProgress, Submitted, Graded }

    // Roadmap / Content
    public enum NodeDifficulty { Beginner, Intermediate, Advanced }
    public enum ContentType { Video, Article, Book, Course, Exercise, Quiz, Project }
    public enum EdgeType { Prerequisite, Recommended, Next }

    // Notification
    public enum NotificationType { System, Reminder, Achievement, Resurvey, AiRecommendation }
    public enum NotificationRelatedType { None, Task, Module, Plan, Node, Session, Roadmap }

    // AI (if you keep AI messages in SQL)
    public enum AiMessageRole { User, Assistant, System }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
