using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAttempts.Common;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Enums;

namespace SSS.Application.Features.QuizAttempts.CreateQuizAttempt
{
    public class CreateQuizAttemptHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<CreateQuizAttemptCommand, CreateQuizAttemptResult>
    {
        public async Task<CreateQuizAttemptResult> Handle(CreateQuizAttemptCommand req, CancellationToken ct)
        {
            var dto = req.CreateQuizAttempt;
            var requestedLevel = string.IsNullOrWhiteSpace(dto.Level)
                ? null
                : NormalizeLevel(dto.Level);

            var studyPlanModule = await db.StudyPlanModules
                .AsNoTracking()
                .Where(m => m.Id == dto.StudyPlanModuleId && m.StudyPlan.UserId == dto.UserId)
                .Select(m => new
                {
                    m.Id,
                    m.RoadmapNodeId,
                    m.StudyPlan.RoadmapId
                })
                .FirstOrDefaultAsync(ct);

            if (studyPlanModule is null)
            {
                throw new KeyNotFoundException($"Study plan module with id {dto.StudyPlanModuleId} not found for this user.");
            }

            var currentLevel = await db.UserLearningTargets
                .AsNoTracking()
                .Where(t =>
                    t.UserId == dto.UserId
                    && t.RoadmapId == studyPlanModule.RoadmapId
                    && t.Status == TargetStatus.active)
                .OrderByDescending(t => t.SnapshotAt)
                .ThenByDescending(t => t.CreatedAt)
                .ThenByDescending(t => t.Id)
                .Select(t => t.CurrentLevel)
                .FirstOrDefaultAsync(ct);

            if (string.IsNullOrWhiteSpace(currentLevel))
            {
                throw new KeyNotFoundException(
                    $"No active learning target found for user {dto.UserId} and roadmap {studyPlanModule.RoadmapId}.");
            }

            var normalizedLevel = requestedLevel ?? NormalizeLevel(currentLevel);

            var quiz = await db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qq => qq.Options)
                .FirstOrDefaultAsync(q =>
                    q.RoadmapNodeId == studyPlanModule.RoadmapNodeId
                    && q.Level.ToLower() == normalizedLevel.ToLower(), ct);

            if (quiz is null)
            {
                throw new KeyNotFoundException(
                    $"No quiz found for module id {dto.StudyPlanModuleId} and level {normalizedLevel}.");
            }

            var hasActiveOrPassedAttempt = await db.QuizAttempts
                .AsNoTracking()
                .AnyAsync(a =>
                    a.QuizId == quiz.Id
                    && a.UserId == dto.UserId
                    && (a.Status == Domain.Enums.QuizAttemptStatus.InProgress
                        || a.Status == Domain.Enums.QuizAttemptStatus.Passed), ct);

            if (hasActiveOrPassedAttempt)
            {
                throw new InvalidOperationException(
                    "A quiz attempt for this module and level already exists with status InProgress or Passed.");
            }

            var quizAttempt = new QuizAttempt
            {
                QuizId = quiz.Id,
                UserId = dto.UserId,
                StartedAt = DateTime.UtcNow,
                Status = Domain.Enums.QuizAttemptStatus.InProgress
            };

            await db.QuizAttempts.AddAsync(quizAttempt, ct);
            await db.SaveChangesAsync(ct);

            var resultDto = mapper.Map<QuizAttemptDto>(quizAttempt);

            var totalQuestionCount = quiz.Questions.Count;
            var questionCount = totalQuestionCount < 10
                ? Math.Min(5, totalQuestionCount)
                : Math.Min(10, totalQuestionCount);

            var randomQuestions = quiz.Questions
                .OrderBy(_ => Guid.NewGuid())
                .Take(questionCount)
                .ToList();

            var quizAnswers = randomQuestions
                .Select(question => new QuizAnswer
                {
                    AttemptId = quizAttempt.Id,
                    QuestionId = question.Id,
                    OptionId = null,
                    AnsweredAt = DateTime.UtcNow
                })
                .ToList();

            await db.QuizAnswers.AddRangeAsync(quizAnswers, ct);
            await db.SaveChangesAsync(ct);

            return new CreateQuizAttemptResult(true, "Quiz attempt created successfully", resultDto);
        }

        private static string NormalizeLevel(string level)
        {
            var value = level.Trim();

            if (value.Equals("Begineer", StringComparison.OrdinalIgnoreCase)
                || value.Equals("Beginner", StringComparison.OrdinalIgnoreCase))
            {
                return "Beginner";
            }

            if (value.Equals("Intermediate", StringComparison.OrdinalIgnoreCase))
            {
                return "Intermediate";
            }

            if (value.Equals("Advanced", StringComparison.OrdinalIgnoreCase))
            {
                return "Advanced";
            }

            return value;
        }
    }
}
