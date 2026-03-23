using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.QuizQuestions.CreateQuizQuestions
{
    public class CreateQuizQuestionsHandler(IAppDbContext db, IMapper mapper)
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

            var questionEntities = mapper.Map<List<QuizQuestion>>(req.CreateQuizQuestionDtos);

            await db.QuizQuestions.AddRangeAsync(questionEntities, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            var resultDtos = mapper.Map<List<CreateQuizQuestionWithOptionsDto>>(questionEntities);

            return new CreateQuizQuestionsResult(resultDtos);
        }
    }
}
