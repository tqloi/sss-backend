using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAttempts.Common;
using SSS.Domain.Entities.Assessment;

namespace SSS.Application.Features.QuizAttempts.CreateQuizAttempt
{
    public class CreateQuizAttemptHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<CreateQuizAttemptCommand, CreateQuizAttemptResult>
    {
        public async Task<CreateQuizAttemptResult> Handle(CreateQuizAttemptCommand req, CancellationToken ct)
        {
            var dto = req.CreateQuizAttempt;
            var normalizedLevel = NormalizeLevel(dto.Level);

            var studyPlanModule = await db.StudyPlanModules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == dto.StudyPlanModuleId, ct);

            if (studyPlanModule is null)
            {
                throw new KeyNotFoundException($"Study plan module with id {dto.StudyPlanModuleId} not found.");
            }

            var quiz = await db.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qq => qq.Options)
                .FirstOrDefaultAsync(q =>
                    q.RoadmapNodeId == studyPlanModule.RoadmapNodeId
                    && q.Level.ToLower() == normalizedLevel.ToLower(), ct);

            if (quiz is null)
            {
                throw new KeyNotFoundException(
                    $"No quiz found for module id {dto.StudyPlanModuleId} and level {normalizedLevel}.");
            }

            var quizAttempt = new QuizAttempt
            {
                QuizId = quiz.Id,
                UserId = dto.UserId,
                StartedAt = DateTime.UtcNow,
                Status = Domain.Enums.QuizAttemptStatus.InProgress
            };

            await db.QuizAttempts.AddAsync(quizAttempt, ct);
            await db.SaveChangesAsync(ct);

            var resultDto = mapper.Map<QuizAttemptDto>(quizAttempt);

            var randomQuestions = quiz.Questions
                .OrderBy(_ => Guid.NewGuid())
                .Take(10)
                .Select(question => new CreateQuizAttemptQuestionDto
                {
                    QuestionId = question.Id,
                    Prompt = question.Prompt,
                    Type = question.Type,
                    OrderNo = question.OrderNo,
                    Options = question.Options
                        .OrderBy(o => o.OrderNo)
                        .Select(option => new CreateQuizAttemptQuestionOptionDto
                        {
                            OptionId = option.Id,
                            ValueKey = option.ValueKey,
                            DisplayText = option.DisplayText,
                            OrderNo = option.OrderNo
                        })
                        .ToList()
                })
                .ToList();

            return new CreateQuizAttemptResult(resultDto, randomQuestions);
        }

        private static string NormalizeLevel(string level)
        {
            var value = level.Trim();

            if (value.Equals("Begineer", StringComparison.OrdinalIgnoreCase)
                || value.Equals("Beginner", StringComparison.OrdinalIgnoreCase))
            {
                return "Beginner";
            }

            if (value.Equals("Intermediate", StringComparison.OrdinalIgnoreCase))
            {
                return "Intermediate";
            }

            if (value.Equals("Advanced", StringComparison.OrdinalIgnoreCase))
            {
                return "Advanced";
            }

            return value;
        }
    }
}
