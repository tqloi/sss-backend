using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.QuizQuestions.CreateQuizQuestions
{
    public class CreateQuizQuestionsHandler(IAppDbContext db)
        : IRequestHandler<CreateQuizQuestionsCommand, CreateQuizQuestionsResult>
    {
        public async Task<CreateQuizQuestionsResult> Handle(CreateQuizQuestionsCommand req, CancellationToken cancellationToken)
        {
            var quizIds = req.CreateQuizQuestionDtos.Select(x => x.QuizId).Distinct().ToList();
            var existingQuizIds = await db.Quizzes
                .Where(x => quizIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var missingQuizIds = quizIds.Except(existingQuizIds).ToList();
            if (missingQuizIds.Count > 0)
            {
                throw new KeyNotFoundException($"Quiz not found: {string.Join(", ", missingQuizIds)}");
            }

            var duplicateQuestionKeys = req.CreateQuizQuestionDtos
                .GroupBy(x => new { x.QuizId, x.QuestionKey })
                .Where(g => g.Count() > 1)
                .Select(g => $"QuizId={g.Key.QuizId}, QuestionKey={g.Key.QuestionKey}")
                .ToList();

            if (duplicateQuestionKeys.Count > 0)
            {
                throw new InvalidOperationException($"Duplicate QuestionKey in request: {string.Join(" | ", duplicateQuestionKeys)}");
            }

            var questionEntities = req.CreateQuizQuestionDtos.Select(dto => new QuizQuestion
            {
                QuizId = dto.QuizId,
                QuestionKey = dto.QuestionKey,
                Prompt = dto.Prompt,
                Type = dto.Type,
                ScoreWeight = dto.ScoreWeight,
                OrderNo = dto.OrderNo,
                IsRequired = dto.IsRequired,
                Options = dto.Options.Select(o => new QuizQuestionOption
                {
                    ValueKey = o.ValueKey,
                    DisplayText = o.DisplayText,
                    IsCorrect = o.IsCorrect,
                    ScoreValue = o.ScoreValue,
                    OrderNo = o.OrderNo
                }).ToList()
            }).ToList();

            await db.QuizQuestions.AddRangeAsync(questionEntities, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            var resultDtos = questionEntities.Select(q => new CreateQuizQuestionWithOptionsDto
            {
                QuizId = q.QuizId,
                QuestionKey = q.QuestionKey,
                Prompt = q.Prompt,
                Type = q.Type,
                ScoreWeight = q.ScoreWeight,
                OrderNo = q.OrderNo,
                IsRequired = q.IsRequired,
                Options = q.Options.Select(o => new CreateQuizQuestionOptionInputDto
                {
                    ValueKey = o.ValueKey,
                    DisplayText = o.DisplayText,
                    IsCorrect = o.IsCorrect,
                    ScoreValue = o.ScoreValue,
                    OrderNo = o.OrderNo
                }).ToList()
            }).ToList();

            return new CreateQuizQuestionsResult(resultDtos);
        }
    }
}
