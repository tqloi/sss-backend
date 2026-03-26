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

            var quizAttempt = await db.QuizAttempts
                .Include(qa => qa.Quiz)
                    .ThenInclude(q => q.Questions)
                        .ThenInclude(qq => qq.Options)
                .FirstOrDefaultAsync(qa => qa.Id == dto.Id, ct);

            if (quizAttempt is null)
            {
                throw new KeyNotFoundException($"Quiz attempt with id {dto.Id} not found.");
            }

            decimal totalScore = 0;
            var submittedAt = DateTime.UtcNow;

            var submittedAnswers = dto.Answers
                .GroupBy(a => a.QuestionId)
                .ToDictionary(g => g.Key, g => g.Last());

            var existingAnswers = await db.QuizAnswers
                .Where(a => a.AttemptId == quizAttempt.Id)
                .ToListAsync(ct);

            if (existingAnswers.Count > 0)
            {
                db.QuizAnswers.RemoveRange(existingAnswers);
            }

            var questionReviews = new List<QuizAttemptQuestionReviewDto>();

            var quizQuestions = quizAttempt.Quiz.Questions
                .OrderBy(q => q.OrderNo)
                .ToList();

            foreach (var question in quizQuestions)
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

                    await db.QuizAnswers.AddAsync(quizAnswer, ct);
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

            quizAttempt.SubmittedAt = submittedAt;
            quizAttempt.Score = totalScore;
            quizAttempt.Status = totalScore >= quizAttempt.Quiz.PassingScore
                ? QuizAttemptStatus.Passed
                : QuizAttemptStatus.Failed;

            db.QuizAttempts.Update(quizAttempt);
            await db.SaveChangesAsync(ct);

            var resultDto = mapper.Map<QuizAttemptDto>(quizAttempt);

            return new SubmitQuizAttemptResult(resultDto, questionReviews);
        }
    }
}
