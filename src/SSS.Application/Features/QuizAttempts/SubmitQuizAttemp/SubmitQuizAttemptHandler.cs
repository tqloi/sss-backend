using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Background;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Application.Features.QuizAttempts.Common;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Enums;
using System.Text.Json;

namespace SSS.Application.Features.QuizAttempts.SubmitQuizAttemp
{
    public class SubmitQuizAttemptHandler(
        IAppDbContext db,
        IMapper mapper,
        IModuleJobDispatcher moduleJobDispatcher,
        INotificationService notificationService,
        ILogger<SubmitQuizAttemptHandler> logger)
        : IRequestHandler<SubmitQuizAttemptCommand, SubmitQuizAttemptResult>
    {
        public async Task<SubmitQuizAttemptResult> Handle(SubmitQuizAttemptCommand req, CancellationToken ct)
        {
            var dto = req.SubmitQuizAttempt;
            var submittedQuestionIds = dto.Answers
                .Select(a => a.QuestionId)
                .Distinct()
                .ToList();

            var attemptInfo = await db.QuizAttempts
                .Where(qa => qa.Id == dto.Id)
                .Select(qa => new
                {
                    QuizAttempt = qa,
                    qa.UserId,
                    PassingScore = qa.Quiz.PassingScore,
                    RoadmapNodeId = qa.Quiz.RoadmapNodeId
                })
                .FirstOrDefaultAsync(ct);

            if (attemptInfo is null)
            {
                throw new KeyNotFoundException($"Quiz attempt with id {dto.Id} not found.");
            }

            var quizAttempt = attemptInfo.QuizAttempt;

            var attemptQuestions = await db.QuizQuestions
                .AsNoTracking()
                .Where(q => q.QuizId == quizAttempt.QuizId && submittedQuestionIds.Contains(q.Id))
                .Include(q => q.Options)
                .OrderBy(q => q.OrderNo)
                .ToListAsync(ct);

            if (attemptQuestions.Count != submittedQuestionIds.Count)
            {
                var loadedQuestionIds = attemptQuestions.Select(q => q.Id).ToHashSet();
                var invalidQuestionIds = submittedQuestionIds
                    .Where(questionId => !loadedQuestionIds.Contains(questionId))
                    .ToList();

                throw new InvalidOperationException(
                    $"Submitted questions do not belong to quiz attempt: {string.Join(", ", invalidQuestionIds)}");
            }

            decimal totalScore = 0;
            var submittedAt = DateTime.UtcNow;
            var questionScore = attemptQuestions.Count == 0
                ? 0m
                : 10m / attemptQuestions.Count;

            var submittedAnswers = dto.Answers
                .GroupBy(a => a.QuestionId)
                .ToDictionary(g => g.Key, g => g.Last());

            var questionReviews = new List<QuizAttemptQuestionReviewDto>();
            var answerEntities = new List<QuizAnswer>(attemptQuestions.Count);

            await db.BeginTransactionAsync(ct);

            try
            {
                await db.QuizAnswers
                    .Where(a => a.AttemptId == quizAttempt.Id)
                    .ExecuteDeleteAsync(ct);

                foreach (var question in attemptQuestions)
                {
                    submittedAnswers.TryGetValue(question.Id, out var answerDto);

                    var correctOptions = question.Options
                        .Where(o => o.IsCorrect)
                        .OrderBy(o => o.OrderNo)
                        .ToList();

                    var selectedOptionIds = GetSelectedOptionIds(answerDto);
                    var selectedOptionIdSet = selectedOptionIds.Count == 0
                        ? null
                        : selectedOptionIds.ToHashSet();
                    var selectedOptions = selectedOptionIds.Count == 0
                        ? new List<QuizQuestionOption>()
                        : question.Options
                            .Where(o => selectedOptionIdSet!.Contains(o.Id))
                            .OrderBy(o => o.OrderNo)
                            .ToList();
                    var persistedSelectedOptionIds = selectedOptions
                        .Select(option => option.Id)
                        .Distinct()
                        .ToList();
                    var selectedOptionTexts = selectedOptions
                        .Select(o => o.DisplayText)
                        .ToList();
                    var correctOptionTexts = correctOptions
                        .Select(o => o.DisplayText)
                        .ToList();

                    var selectedTextValue = answerDto?.TextValue?.Trim();
                    var correctTextValue = correctOptions.Count == 0
                        ? null
                        : string.Join(" | ", correctOptions.Select(o => o.DisplayText));

                    var isCorrect = IsCorrectAnswer(question.Type, selectedOptions, selectedTextValue, correctOptions);

                    if (isCorrect)
                    {
                        totalScore += questionScore;
                    }

                    if (answerDto is not null)
                    {
                        var storedTextValue = question.Type == QuizQuestionType.MultipleChoice
                            ? (persistedSelectedOptionIds.Count > 0 ? JsonSerializer.Serialize(persistedSelectedOptionIds) : null)
                            : answerDto.TextValue;

                        var quizAnswer = new QuizAnswer
                        {
                            AttemptId = quizAttempt.Id,
                            QuestionId = question.Id,
                            OptionId = persistedSelectedOptionIds.Count > 0
                                ? persistedSelectedOptionIds.First()
                                : null,
                            TextValue = storedTextValue,
                            NumberValue = answerDto.NumberValue,
                            AnsweredAt = submittedAt,
                            ScoredValue = isCorrect
                                ? questionScore : 0m
                        };

                        answerEntities.Add(quizAnswer);
                    }

                    questionReviews.Add(new QuizAttemptQuestionReviewDto
                    {
                        QuestionId = question.Id,
                        Prompt = question.Prompt,
                        Type = question.Type,
                        SelectedOptionId = persistedSelectedOptionIds.Count > 0
                            ? persistedSelectedOptionIds.First()
                            : null,
                        SelectedOptionIds = persistedSelectedOptionIds,
                        SelectedOptionTexts = selectedOptionTexts,
                        SelectedTextValue = selectedTextValue,
                        SelectedOptionText = selectedOptionTexts.Count == 0
                            ? null
                            : string.Join(" | ", selectedOptionTexts),
                        CorrectOptionId = correctOptions.FirstOrDefault()?.Id,
                        CorrectOptionIds = correctOptions.Select(o => o.Id).ToList(),
                        CorrectOptionTexts = correctOptionTexts,
                        CorrectTextValue = correctTextValue,
                        CorrectOptionText = correctOptionTexts.Count == 0
                            ? null
                            : string.Join(" | ", correctOptionTexts),
                        IsCorrect = isCorrect
                    });
                }

                if (answerEntities.Count > 0)
                {
                    await db.QuizAnswers.AddRangeAsync(answerEntities, ct);
                }

                quizAttempt.SubmittedAt = submittedAt;
                quizAttempt.Score = totalScore;
                quizAttempt.Status = totalScore >= attemptInfo.PassingScore
                    ? QuizAttemptStatus.Passed
                    : QuizAttemptStatus.Failed;

                await db.SaveChangesAsync(ct);
                await db.CommitTransactionAsync(ct);
            }
            catch
            {
                await db.RollbackTransactionAsync(ct);
                throw;
            }

            if (quizAttempt.Status == QuizAttemptStatus.Passed)
            {
                var completedModuleInfo = await db.StudyPlanModules
                    .AsNoTracking()
                    .Where(m =>
                        m.RoadmapNodeId == attemptInfo.RoadmapNodeId
                        && m.StudyPlan.UserId == attemptInfo.UserId)
                    .OrderByDescending(m => m.Id)
                    .Select(m => new
                    {
                        ModuleId = (long?)m.Id,
                        m.StudyPlanId,
                        ModuleName = m.RoadmapNode.Title
                    })
                    .FirstOrDefaultAsync(ct);

                if (completedModuleInfo?.ModuleId is long moduleId)
                {
                    try
                    {
                        moduleJobDispatcher.DispatchCompleteModule((int)moduleId);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(
                            ex,
                            "[SubmitQuizAttempt] Failed to enqueue CompleteModule job for moduleId={ModuleId}",
                            moduleId);
                    }

                    try
                    {
                        await notificationService.CreateAndDispatchAsync(
                            userId: attemptInfo.UserId,
                            title: "Module completed",
                            content: $"You have completed module '{completedModuleInfo.ModuleName}'. Great progress!",
                            type: NotificationType.Achievement,
                            relatedType: NotificationRelatedType.Module,
                            relatedId: moduleId,
                            status: ModuleStatus.Completed.ToString(),
                            actionUrl: $"/study-plans/{completedModuleInfo.StudyPlanId}",
                            dedupeKey: $"moduleCompleted:{completedModuleInfo.StudyPlanId}:{moduleId}",
                            isPush: false,
                            ct: ct);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(
                            ex,
                            "[SubmitQuizAttempt] Failed to dispatch immediate module-completed notification for moduleId={ModuleId}",
                            moduleId);
                    }
                }
            }

            var resultDto = mapper.Map<QuizAttemptDto>(quizAttempt);

            return new SubmitQuizAttemptResult(resultDto, questionReviews);
        }

        private static List<long> GetSelectedOptionIds(SubmitQuizAttemptAnswerDto? answerDto)
        {
            if (answerDto is null)
            {
                return new List<long>();
            }

            var optionIds = new List<long>();

            if (answerDto.OptionIds.Count > 0)
            {
                optionIds.AddRange(answerDto.OptionIds.Where(optionId => optionId > 0));
            }

            if (answerDto.OptionId.HasValue && answerDto.OptionId.Value > 0)
            {
                optionIds.Add(answerDto.OptionId.Value);
            }

            return optionIds.Distinct().ToList();
        }

        private static bool IsCorrectAnswer(
            QuizQuestionType questionType,
            List<QuizQuestionOption> selectedOptions,
            string? selectedTextValue,
            List<QuizQuestionOption> correctOptions)
        {
            if (correctOptions.Count == 0)
            {
                return false;
            }

            return questionType switch
            {
                QuizQuestionType.SingleChoice =>
                    selectedOptions.Count == 1
                    && correctOptions.Count == 1
                    && selectedOptions[0].Id == correctOptions[0].Id,

                QuizQuestionType.MultipleChoice =>
                    selectedOptions.Count > 0
                    && selectedOptions.Select(option => option.Id).ToHashSet()
                        .SetEquals(correctOptions.Select(option => option.Id)),

                QuizQuestionType.ShortAnswer =>
                    !string.IsNullOrWhiteSpace(selectedTextValue)
                    && correctOptions.Any(option => TextMatches(selectedTextValue, option.DisplayText) || TextMatches(selectedTextValue, option.ValueKey)),

                _ => false
            };
        }

        private static bool TextMatches(string left, string right)
        {
            return string.Equals(
                left.Trim(),
                right.Trim(),
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
