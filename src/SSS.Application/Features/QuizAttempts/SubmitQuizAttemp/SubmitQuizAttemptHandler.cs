using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAttempts.Common;
using SSS.Domain.Entities.Assessment;
using SSS.Domain.Enums;

namespace SSS.Application.Features.QuizAttempts.SubmitQuizAttemp
{
    public class SubmitQuizAttemptHandler(IAppDbContext db, IMapper mapper)
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
                .Select(q => new { q.Id, q.PassingScore })
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

                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                var selectedOption = answerDto?.OptionId.HasValue == true
                    ? question.Options.FirstOrDefault(o => o.Id == answerDto.OptionId.Value)
                    : null;

                var isCorrect = correctOption is not null
                    && selectedOption is not null
                    && selectedOption.Id == correctOption.Id;

                var selectedOptionScore = selectedOption?.ScoreValue ?? 0m;

                if (isCorrect && selectedOption != null)
                {
                    totalScore += selectedOptionScore;
                }

                if (answerDto is not null)
                {
                    var quizAnswer = new QuizAnswer
                    {
                        AttemptId = quizAttempt.Id,
                        QuestionId = question.Id,
                        OptionId = answerDto.OptionId,
                        TextValue = answerDto.TextValue,
                        NumberValue = answerDto.NumberValue,
                        AnsweredAt = submittedAt,
                        ScoredValue = isCorrect
                            ? selectedOptionScore : 0m
                    };

                            answerEntities.Add(quizAnswer);
                }

                questionReviews.Add(new QuizAttemptQuestionReviewDto
                {
                    QuestionId = question.Id,
                    Prompt = question.Prompt,
                    SelectedOptionId = selectedOption?.Id,
                    SelectedOptionText = selectedOption?.DisplayText,
                    CorrectOptionId = correctOption?.Id,
                    CorrectOptionText = correctOption?.DisplayText,
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

            var resultDto = mapper.Map<QuizAttemptDto>(quizAttempt);

            return new SubmitQuizAttemptResult(resultDto, questionReviews);
        }
    }
}
