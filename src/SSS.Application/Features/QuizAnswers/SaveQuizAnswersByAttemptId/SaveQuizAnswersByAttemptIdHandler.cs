using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.QuizAnswers.SaveQuizAnswersByAttemptId
{
    public class SaveQuizAnswersByAttemptIdHandler(IAppDbContext db)
        : IRequestHandler<SaveQuizAnswersByAttemptIdCommand, SaveQuizAnswersByAttemptIdResult>
    {
        public async Task<SaveQuizAnswersByAttemptIdResult> Handle(
            SaveQuizAnswersByAttemptIdCommand req, 
            CancellationToken ct)
        {
            var attemptExists = await db.QuizAttempts
                .AsNoTracking()
                .AnyAsync(qa => qa.Id == req.AttemptId, ct);

            if (!attemptExists)
            {
                throw new KeyNotFoundException($"Quiz attempt with id {req.AttemptId} not found.");
            }

            var answers = req.QuizAnswers
                .Where(qa => qa.AttemptId == req.AttemptId)
                .ToList();

            if (answers.Count == 0)
            {
                throw new InvalidOperationException("No valid quiz answers to update.");
            }

            var answerIds = answers.Select(a => a.Id).ToList();
            var existingAnswers = await db.QuizAnswers
                .Where(qa => answerIds.Contains(qa.Id))
                .ToListAsync(ct);

            foreach (var existingAnswer in existingAnswers)
            {
                var updatedAnswer = answers.FirstOrDefault(a => a.Id == existingAnswer.Id);
                if (updatedAnswer != null)
                {
                    existingAnswer.OptionId = updatedAnswer.OptionId;
                    existingAnswer.TextValue = updatedAnswer.TextValue;
                    existingAnswer.NumberValue = updatedAnswer.NumberValue;
                    existingAnswer.AnsweredAt = DateTime.UtcNow;
                }
            }

            db.QuizAnswers.UpdateRange(existingAnswers);
            await db.SaveChangesAsync(ct);

            return new SaveQuizAnswersByAttemptIdResult(true, existingAnswers.Count);
        }
    }
}
