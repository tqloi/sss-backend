using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAnswers.Common;
using SSS.Domain.Entities.Assessment;
using System.Text.Json;

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
                .Where(qa => qa.QuestionId > 0)
                .ToList();

            if (answers.Count == 0)
            {
                throw new InvalidOperationException("No valid quiz answers to update.");
            }

            var questionIds = answers.Select(a => a.QuestionId).Distinct().ToList();
            var existingAnswers = await db.QuizAnswers
                .Where(qa => qa.AttemptId == req.AttemptId && questionIds.Contains(qa.QuestionId))
                .ToListAsync(ct);

            if (existingAnswers.Count == 0)
            {
                throw new KeyNotFoundException(
                    "No quiz answers found for the provided questions in this attempt.");
            }

            var answerByQuestionId = answers
                .GroupBy(a => a.QuestionId)
                .ToDictionary(g => g.Key, g => g.Last());

            foreach (var existingAnswer in existingAnswers)
            {
                if (answerByQuestionId.TryGetValue(existingAnswer.QuestionId, out var updatedAnswer))
                {
                    var selectedOptionIds = NormalizeSelectedOptionIds(updatedAnswer);

                    existingAnswer.OptionId = selectedOptionIds.FirstOrDefault();
                    existingAnswer.TextValue = selectedOptionIds.Count > 1
                        ? JsonSerializer.Serialize(selectedOptionIds)
                        : updatedAnswer.TextValue;
                    existingAnswer.NumberValue = updatedAnswer.NumberValue;
                    existingAnswer.AnsweredAt = updatedAnswer.AnsweredAt ?? DateTime.UtcNow;
                }
            }

            db.QuizAnswers.UpdateRange(existingAnswers);
            await db.SaveChangesAsync(ct);

            return new SaveQuizAnswersByAttemptIdResult(true, existingAnswers.Count);
        }

        private static List<long> NormalizeSelectedOptionIds(SaveQuizAnswerByQuestionDto updatedAnswer)
        {
            var selectedOptionIds = new List<long>();

            if (updatedAnswer.OptionIds.Count > 0)
            {
                selectedOptionIds.AddRange(updatedAnswer.OptionIds.Where(optionId => optionId > 0));
            }

            if (updatedAnswer.OptionId.HasValue && updatedAnswer.OptionId.Value > 0)
            {
                selectedOptionIds.Add(updatedAnswer.OptionId.Value);
            }

            return selectedOptionIds.Distinct().ToList();
        }
    }
}
