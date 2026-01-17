using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SSS.Domain.Entities.Identity;
using SSS.Domain.Entities.Notification;
using SSS.Domain.Entities.Planning;
using SSS.Domain.Entities.Content;
using SSS.Domain.Entities.Tracking;
using SSS.Domain.Entities.Assessment;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Infrastructure.Persistence.Sql
{
    public class AppDbContext : IdentityDbContext<User>, IAppDbContext
    {
        public DbSet<LearningCategory> LearningCategories => Set<LearningCategory>();
        public DbSet<LearningSubject> LearningSubjects => Set<LearningSubject>();
        public DbSet<Roadmap> Roadmaps => Set<Roadmap>();
        public DbSet<RoadmapNode> RoadmapNodes => Set<RoadmapNode>();
        public DbSet<RoadmapEdge> RoadmapEdges => Set<RoadmapEdge>();
        public DbSet<NodeContent> NodeContents => Set<NodeContent>();

        // Survey & Profile
        public DbSet<Survey> Surveys => Set<Survey>();
        public DbSet<SurveyQuestion> SurveyQuestions => Set<SurveyQuestion>();
        public DbSet<SurveyQuestionOption> SurveyQuestionOptions => Set<SurveyQuestionOption>();
        public DbSet<SurveyResponse> SurveyResponses => Set<SurveyResponse>();
        public DbSet<SurveyAnswer> SurveyAnswers => Set<SurveyAnswer>();
        public DbSet<UserLearningProfile> UserLearningProfiles => Set<UserLearningProfile>();

        // Study Plan / Tasks / Tracking
        public DbSet<StudyPlan> StudyPlans => Set<StudyPlan>();
        public DbSet<StudyPlanModule> StudyPlanModules => Set<StudyPlanModule>();
        public DbSet<TaskItem> TaskItems => Set<TaskItem>();
        public DbSet<StudySession> StudySessions => Set<StudySession>();

        // Progress / Behavior
        public DbSet<UserSubjectStat> UserSubjectStats => Set<UserSubjectStat>();
        public DbSet<UserGamification> UserGamifications => Set<UserGamification>();
        public DbSet<UserBehaviorWindow> UserBehaviorWindows => Set<UserBehaviorWindow>();

        // Quiz
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
        public DbSet<QuizQuestionOption> QuizQuestionOptions => Set<QuizQuestionOption>();
        public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
        public DbSet<QuizAnswer> QuizAnswers => Set<QuizAnswer>();

        // Notification
        public DbSet<UserNotification> UserNotifications => Set<UserNotification>();
        public DbSet<UserPushToken> UserPushTokens => Set<UserPushToken>();

        // Security (non-default identity table)
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder cb)
        {
            // enum
            cb.Properties<Enum>().HaveConversion<string>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            ConfigureTableNames(builder: builder);
        }

        private static void ConfigureTableNames(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                var entityNamespace = entityType.ClrType.Namespace ?? "";

                // 1. Xóa Schema (Bắt buộc cho MySQL)
                entityType.SetSchema(null);

                // 2. Xác định Prefix dựa trên Namespace folder
                string prefix = entityNamespace switch
                {
                    var ns when ns.Contains("Identity") => "Id_",
                    var ns when ns.Contains("Content") => "Ct_",
                    var ns when ns.Contains("Assessment") => "As_",
                    var ns when ns.Contains("Planning") => "Pl_",
                    var ns when ns.Contains("Tracking") => "Tr_",
                    var ns when ns.Contains("Notification") => "Nt_", // Sửa lại đúng tên folder bạn đặt
                    _ => ""
                };

                // 3. Xử lý đổi tên
                if (tableName != null && tableName.StartsWith("AspNet"))
                {
                    // Identity mặc định: AspNetUsers -> Id_Users
                    entityType.SetTableName("Id_" + tableName[6..]);
                }
                else if (!string.IsNullOrEmpty(prefix) && tableName != null && !tableName.StartsWith(prefix))
                {
                    // Các entity của bạn: LearningSubject -> Ct_LearningSubject
                    entityType.SetTableName(prefix + tableName);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
         => await base.SaveChangesAsync(ct);

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
            => await Database.BeginTransactionAsync(ct);

        public async Task CommitTransactionAsync(CancellationToken ct = default)
            => await Database.CommitTransactionAsync(ct);

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
            => await Database.RollbackTransactionAsync(ct);
    }
}
