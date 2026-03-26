using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.GetQuizzesByQuizAttempt
{
    public class GetQuizzesByQuizAttemptHandler(IAppDbContext db)
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

            var quizAnswers = await db.QuizAnswers
                .AsNoTracking()
                .Where(qa => qa.AttemptId == req.AttemptId)
                .ToListAsync(ct);

            var answeredQuestionIds = quizAnswers
                .Select(qa => qa.QuestionId)
                .Distinct()
                .ToList();

            var questions = await db.QuizQuestions
                .AsNoTracking()
                .Where(q => q.QuizId == quizAttempt.QuizId && answeredQuestionIds.Contains(q.Id))
                .Include(q => q.Options)
                .OrderBy(q => q.OrderNo)
                .ToListAsync(ct);

            var selectedOptionsByQuestion = quizAnswers
                .GroupBy(qa => qa.QuestionId)
                .ToDictionary(g => g.Key, g => g.First().OptionId);

            var result = questions.Select(q => new QuizQuestionWithAnswerDto
            {
                QuestionId = q.Id,
                Prompt = q.Prompt,
                OrderNo = q.OrderNo,
                SelectedOptionId = selectedOptionsByQuestion.TryGetValue(q.Id, out var optionId)
                    ? optionId
                    : null,
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

            return new GetQuizzesByQuizAttemptResult(quiz, result);
        }
    }
}
