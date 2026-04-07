using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Application.Features.QuizAttempts.Common;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Enums;
using System.Text.Json;

namespace SSS.Application.Features.QuizAttempts.SubmitQuizAttemp
{
    public class SubmitQuizAttemptHandler(IAppDbContext db, IMapper mapper, IModuleService moduleService)
        : IRequestHandler<SubmitQuizAttemptCommand, SubmitQuizAttemptResult>
    {
        public async Task<SubmitQuizAttemptResult> Handle(SubmitQuizAttemptCommand req, CancellationToken ct)
        {
            var dto = req.SubmitQuizAttempt;
            var submittedQuestionIds = dto.Answers
                .Select(a => a.QuestionId)
                .Distinct()
                .ToList();

            var quizAttempt = await db.QuizAttempts
                .FirstOrDefaultAsync(qa => qa.Id == dto.Id, ct);

            if (quizAttempt is null)
            {
                throw new KeyNotFoundException($"Quiz attempt with id {dto.Id} not found.");
            }

            var quiz = await db.Quizzes
                .AsNoTracking()
                .Select(q => new { q.Id, q.PassingScore, q.RoadmapNodeId })
                .FirstOrDefaultAsync(q => q.Id == quizAttempt.QuizId, ct);

            if (quiz is null)   
            {
                throw new KeyNotFoundException($"Quiz with id {quizAttempt.QuizId} not found.");
            }

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

            await db.QuizAnswers
                .Where(a => a.AttemptId == quizAttempt.Id)
                .ExecuteDeleteAsync(ct);

            var questionReviews = new List<QuizAttemptQuestionReviewDto>();
            var answerEntities = new List<QuizAnswer>(attemptQuestions.Count);

            foreach (var question in attemptQuestions)
            {
                submittedAnswers.TryGetValue(question.Id, out var answerDto);

                var correctOptions = question.Options
                    .Where(o => o.IsCorrect)
                    .OrderBy(o => o.OrderNo)
                    .ToList();

                var selectedOptionIds = GetSelectedOptionIds(answerDto);
                var selectedOptions = selectedOptionIds.Count == 0
                    ? new List<QuizQuestionOption>()
                    : question.Options.Where(o => selectedOptionIds.Contains(o.Id)).ToList();

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
                        ? (selectedOptionIds.Count > 0 ? JsonSerializer.Serialize(selectedOptionIds) : null)
                        : answerDto.TextValue;

                    var quizAnswer = new QuizAnswer
                    {
                        AttemptId = quizAttempt.Id,
                        QuestionId = question.Id,
                        OptionId = selectedOptionIds.FirstOrDefault(),
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
                    SelectedOptionId = selectedOptions.FirstOrDefault()?.Id,
                    SelectedOptionIds = selectedOptionIds,
                    SelectedTextValue = selectedTextValue,
                    SelectedOptionText = selectedOptions.FirstOrDefault()?.DisplayText,
                    CorrectOptionId = correctOptions.FirstOrDefault()?.Id,
                    CorrectOptionIds = correctOptions.Select(o => o.Id).ToList(),
                    CorrectTextValue = correctTextValue,
                    CorrectOptionText = correctOptions.FirstOrDefault()?.DisplayText,
                    IsCorrect = isCorrect
                });
            }

            if (answerEntities.Count > 0)
            {
                await db.QuizAnswers.AddRangeAsync(answerEntities, ct);
            }

            quizAttempt.SubmittedAt = submittedAt;
            quizAttempt.Score = totalScore;
            quizAttempt.Status = totalScore >= quiz.PassingScore
                ? QuizAttemptStatus.Passed
                : QuizAttemptStatus.Failed;

            await db.SaveChangesAsync(ct);

            if (quizAttempt.Status == QuizAttemptStatus.Passed)
            {
                var studyPlanModuleId = await db.StudyPlanModules
                    .AsNoTracking()
                    .Where(m =>
                        m.RoadmapNodeId == quiz.RoadmapNodeId
                        && m.StudyPlan.UserId == quizAttempt.UserId)
                    .OrderByDescending(m => m.Id)
                    .Select(m => (long?)m.Id)
                    .FirstOrDefaultAsync(ct);

                if (studyPlanModuleId.HasValue)
                {
                    await moduleService.CompleteModuleAsync((int)studyPlanModuleId.Value, ct);
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
                    && selectedOptions.Select(option => option.Id).OrderBy(optionId => optionId)
                        .SequenceEqual(correctOptions.Select(option => option.Id).OrderBy(optionId => optionId)),

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
