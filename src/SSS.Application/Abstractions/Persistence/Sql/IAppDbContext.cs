using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Entities.Content;
using SSS.Domain.Entities.Identity;
using SSS.Domain.Entities.Notification;
using SSS.Domain.Entities.Planning;
using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Abstractions.Persistence.Sql
{
    public interface IAppDbContext
    {
        // Content
        DbSet<LearningCategory> LearningCategories { get; }
        DbSet<LearningSubject> LearningSubjects { get; }
        DbSet<Roadmap> Roadmaps { get; }
        DbSet<RoadmapNode> RoadmapNodes { get; }
        DbSet<RoadmapEdge> RoadmapEdges { get; }
        DbSet<NodeContent> NodeContents { get; }

        // Survey & Profile
        DbSet<Survey> Surveys { get; }
        DbSet<SurveyQuestion> SurveyQuestions { get; }
        DbSet<SurveyQuestionOption> SurveyQuestionOptions { get; }
        DbSet<SurveyResponse> SurveyResponses { get; }
        DbSet<SurveyAnswer> SurveyAnswers { get; }
        DbSet<UserLearningProfile> UserLearningProfiles { get; }

        // Planning / Tracking
        DbSet<StudyPlan> StudyPlans { get; }
        DbSet<StudyPlanModule> StudyPlanModules { get; }
        DbSet<TaskItem> TaskItems { get; }
        DbSet<StudySession> StudySessions { get; }

        // Progress / Behavior
        DbSet<UserSubjectStat> UserSubjectStats { get; }
        DbSet<UserGamification> UserGamifications { get; }
        DbSet<UserBehaviorWindow> UserBehaviorWindows { get; }

        // Quiz
        DbSet<Quiz> Quizzes { get; }
        DbSet<QuizQuestion> QuizQuestions { get; }
        DbSet<QuizQuestionOption> QuizQuestionOptions { get; }
        DbSet<QuizAttempt> QuizAttempts { get; }
        DbSet<QuizAnswer> QuizAnswers { get; }

        // Notification
        DbSet<UserNotification> UserNotifications { get; }
        DbSet<UserPushToken> UserPushTokens { get; }

        // Security
        DbSet<RefreshToken> RefreshTokens { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
