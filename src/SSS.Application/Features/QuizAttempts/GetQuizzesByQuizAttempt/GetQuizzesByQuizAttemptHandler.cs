using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAttempts.Common;
using SSS.Domain.Constants;

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

                var staticQuestions = questions.Select(q => new QuizQuestionWithAnswerDto
                {
                    QuestionId = q.Id,
                    Prompt = q.Prompt,
                    OrderNo = q.OrderNo,
                    SelectedOptionId = null,
                    Options = q.Options
                        .OrderBy(o => o.OrderNo)
                        .Select(o => new QuizOptionWithAnswerDto
                        {
                            OptionId = o.Id,
                            ValueKey = o.ValueKey,
                            DisplayText = o.DisplayText,
                            OrderNo = o.OrderNo
                        })
                        .ToList()
                }).ToList();

                staticPayload = new GetQuizzesByQuizAttemptResult(quiz, staticQuestions);
                await cacheService.SetAsync(staticCacheKey, staticPayload, CacheConstants.DefaultExpiration);
            }

            var quizAnswers = await db.QuizAnswers
                .AsNoTracking()
                .Where(qa => qa.AttemptId == req.AttemptId)
                .ToListAsync(ct);

            var selectedOptionsByQuestion = quizAnswers
                .GroupBy(qa => qa.QuestionId)
                .ToDictionary(g => g.Key, g => g.First().OptionId);

            var result = staticPayload.Questions.Select(q => new QuizQuestionWithAnswerDto
            {
                QuestionId = q.QuestionId,
                Prompt = q.Prompt,
                OrderNo = q.OrderNo,
                SelectedOptionId = selectedOptionsByQuestion.TryGetValue(q.QuestionId, out var optionId)
                    ? optionId
                    : null,
                Options = q.Options
                    .OrderBy(o => o.OrderNo)
                    .Select(o => new QuizOptionWithAnswerDto
                    {
                        OptionId = o.OptionId,
                        ValueKey = o.ValueKey,
                        DisplayText = o.DisplayText,
                        OrderNo = o.OrderNo
                    })
                    .ToList()
            }).ToList();

            return new GetQuizzesByQuizAttemptResult(staticPayload.Quiz, result);
        }
    }
}
