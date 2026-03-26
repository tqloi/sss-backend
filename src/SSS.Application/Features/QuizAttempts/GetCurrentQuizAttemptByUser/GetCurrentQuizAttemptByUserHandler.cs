using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAttempts.Common;

namespace SSS.Application.Features.QuizAttempts.GetCurrentQuizAttemptByUser
{
    public class GetCurrentQuizAttemptByUserHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetCurrentQuizAttemptByUserQuery, GetCurrentQuizAttemptByUserResult>
    {
        public async Task<GetCurrentQuizAttemptByUserResult> Handle(
            GetCurrentQuizAttemptByUserQuery req, 
            CancellationToken ct)
        {
            var studyPlanModule = await db.StudyPlanModules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == req.ModuleId, ct);

            if (studyPlanModule is null)
            {
                throw new KeyNotFoundException($"Study plan module with id {req.ModuleId} not found.");
            }

            var quiz = await db.Quizzes
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.RoadmapNodeId == studyPlanModule.RoadmapNodeId, ct);

            if (quiz is null)
            {
                throw new KeyNotFoundException($"Quiz not found for module {req.ModuleId}.");
            }

            var quizAttempt = await db.QuizAttempts
                .AsNoTracking()
                .FirstOrDefaultAsync(qa =>
                    qa.QuizId == quiz.Id 
                    && qa.UserId == req.UserId 
                    && qa.Status == Domain.Enums.QuizAttemptStatus.InProgress, ct);

            var resultDto = quizAttempt is not null 
                ? mapper.Map<QuizAttemptDto>(quizAttempt) 
                : null;

            return new GetCurrentQuizAttemptByUserResult(resultDto);
        }
    }
}
