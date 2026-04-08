using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAnswers.Common;
using SSS.Application.Features.QuizAttempts.Common;
using SSS.Domain.Constants;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Enums;

namespace SSS.Application.Features.QuizAttempts.GetQuizzesByQuizAttempt
{
    public class GetQuizzesByQuizAttemptHandler(IAppDbContext db, ICacheService cacheService)
        : IRequestHandler<GetQuizzesByQuizAttemptQuery, GetQuizzesByQuizAttemptResult>
    {
        public async Task<GetQuizzesByQuizAttemptResult> Handle(
            GetQuizzesByQuizAttemptQuery req,
            CancellationToken ct)
        {
            var quizAttempt = await db.QuizAttempts
                .AsNoTracking()
                .FirstOrDefaultAsync(qa => qa.Id == req.AttemptId, ct);

            if (quizAttempt is null)
            {
                throw new KeyNotFoundException($"Quiz attempt with id {req.AttemptId} not found.");
            }

            var staticCacheKey = $"quiz-attempt:question-payload:{req.AttemptId}";
            var staticPayload = await cacheService.GetAsync<GetQuizzesByQuizAttemptResult>(staticCacheKey);

            if (staticPayload is null)
            {
                var quiz = await db.Quizzes
                    .AsNoTracking()
                    .Where(q => q.Id == quizAttempt.QuizId)
                    .Select(q => new QuizBasicInfoDto
                    {
                        QuizId = q.Id,
                        Title = q.Title,
                        Description = q.Description,
                        Level = q.Level,
                        TotalScore = q.TotalScore,
                        PassingScore = q.PassingScore
                    })
                    .FirstOrDefaultAsync(ct);

                if (quiz is null)
                {
                    throw new KeyNotFoundException($"Quiz with id {quizAttempt.QuizId} not found.");
                }

                var attemptQuestionIds = await db.QuizAnswers
                    .AsNoTracking()
                    .Where(qa => qa.AttemptId == req.AttemptId)
                    .Select(qa => qa.QuestionId)
                    .Distinct()
                    .ToListAsync(ct);

                var questions = await db.QuizQuestions
                    .AsNoTracking()
                    .Where(q => q.QuizId == quizAttempt.QuizId && attemptQuestionIds.Contains(q.Id))
                    .Include(q => q.Options)
                    .OrderBy(q => q.OrderNo)
                    .ToListAsync(ct);

                var staticQuestions = questions.Select(BuildStaticQuestionDto).ToList();

                staticPayload = new GetQuizzesByQuizAttemptResult(quiz, staticQuestions);
                await cacheService.SetAsync(staticCacheKey, staticPayload, CacheConstants.DefaultExpiration);
            }

            var quizAnswers = await db.QuizAnswers
                .AsNoTracking()
                .Where(qa => qa.AttemptId == req.AttemptId)
                .ToListAsync(ct);

            var selectedAnswersByQuestion = quizAnswers
                .GroupBy(qa => qa.QuestionId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = staticPayload.Questions.Select(q =>
                selectedAnswersByQuestion.TryGetValue(q.QuestionId, out var answersForQuestion)
                    ? ApplySelectedAnswers(q, answersForQuestion)
                    : ApplySelectedAnswers(q, null)).ToList();

            return new GetQuizzesByQuizAttemptResult(staticPayload.Quiz, result);
        }

        private static QuizQuestionWithAnswerDto BuildStaticQuestionDto(QuizQuestion question)
        {
            var options = question.Options
                .OrderBy(o => o.OrderNo)
                .Select(o => new QuizOptionWithAnswerDto
                {
                    OptionId = o.Id,
                    ValueKey = o.ValueKey,
                    DisplayText = o.DisplayText,
                    OrderNo = o.OrderNo
                })
                .ToList();

            var correctOptions = question.Options
                .Where(o => o.IsCorrect)
                .OrderBy(o => o.OrderNo)
                .ToList();

            return new QuizQuestionWithAnswerDto
            {
                QuestionId = question.Id,
                Prompt = question.Prompt,
                Type = question.Type,
                OrderNo = question.OrderNo,
                Options = options,
                CorrectOptionId = correctOptions.FirstOrDefault()?.Id,
                CorrectOptionIds = correctOptions.Select(o => o.Id).ToList(),
                CorrectTextValue = correctOptions.Count == 0
                    ? null
                    : string.Join(" | ", correctOptions.Select(o => o.DisplayText))
            };
        }

        private static QuizQuestionWithAnswerDto ApplySelectedAnswers(
            QuizQuestionWithAnswerDto question,
            List<QuizAnswer>? answersForQuestion)
        {
            var selectedOptionIds = new List<long>();
            string? selectedTextValue = null;

            if (answersForQuestion is not null && answersForQuestion.Count > 0)
            {
                // Use unified extraction for consistent option retrieval
                selectedOptionIds = ExtractAllSelectedOptionIds(question.Type, answersForQuestion);
                selectedTextValue = GetSelectedTextValue(question.Type, answersForQuestion);
            }

            return new QuizQuestionWithAnswerDto
            {
                QuestionId = question.QuestionId,
                Prompt = question.Prompt,
                Type = question.Type,
                OrderNo = question.OrderNo,
                Options = question.Options
                    .OrderBy(o => o.OrderNo)
                    .Select(o => new QuizOptionWithAnswerDto
                    {
                        OptionId = o.OptionId,
                        ValueKey = o.ValueKey,
                        DisplayText = o.DisplayText,
                        OrderNo = o.OrderNo
                    })
                    .ToList(),
                SelectedOptionId = selectedOptionIds.FirstOrDefault(),
                SelectedOptionIds = selectedOptionIds,
                SelectedTextValue = selectedTextValue,
                CorrectOptionId = question.CorrectOptionId,
                CorrectOptionIds = question.CorrectOptionIds,
                CorrectTextValue = question.CorrectTextValue
            };
        }

        /// <summary>
        /// Extracts all selected option IDs from answered questions using unified helper.
        /// Works consistently for both single-choice and multi-choice questions.
        /// </summary>
        private static List<long> ExtractAllSelectedOptionIds(
            QuizQuestionType questionType,
            List<QuizAnswer> answersForQuestion)
        {
            var allOptionIds = new List<long>();

            foreach (var answer in answersForQuestion)
            {
                var optionIds = QuizAnswerExtractionHelper.ExtractSelectedOptionIds(answer);
                allOptionIds.AddRange(optionIds);
            }

            return allOptionIds.Distinct().ToList();
        }

        private static string? GetSelectedTextValue(
            QuizQuestionType questionType,
            List<QuizAnswer> answersForQuestion)
        {
            if (questionType == QuizQuestionType.ShortAnswer)
            {
                return answersForQuestion
                    .Select(answer => answer.TextValue)
                    .LastOrDefault(value => !string.IsNullOrWhiteSpace(value));
            }

            return null;
        }
    }
}
