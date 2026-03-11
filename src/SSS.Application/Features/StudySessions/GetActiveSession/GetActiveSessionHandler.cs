using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.GetActiveSession
{
    public class GetActiveSessionHandler(IAppDbContext context)
        : IRequestHandler<GetActiveSessionQuery, GetActiveSessionResult>
    {
        public async Task<GetActiveSessionResult> Handle(GetActiveSessionQuery req, CancellationToken ct)
        {
            var session = await context.StudySessions
                .AsNoTracking()
                .Include(s => s.Node)
                .Include(s => s.StudyPlan)
                    .ThenInclude(sp => sp!.Roadmap)
                .Where(s => s.UserId == req.UserId &&
                            (s.Status == SessionStatus.InProgress || s.Status == SessionStatus.Paused))
                .OrderByDescending(s => s.StartAt)
                .FirstOrDefaultAsync(ct);

            if (session == null)
            {
                return new GetActiveSessionResult { Success = true, Data = null };
            }

            var elapsedSeconds = (int)(DateTime.UtcNow - session.StartAt).TotalSeconds;

            return new GetActiveSessionResult
            {
                Success = true,
                Data = new ActiveSessionDto
                {
                    SessionId = session.Id,
                    Status = session.Status.ToString(),
                    StartAt = session.StartAt,
                    ElapsedSeconds = elapsedSeconds,
                    PlanId = session.StudyPlanId,
                    NodeId = session.NodeId,
                    NodeTitle = session.Node?.Title,
                    PlanTitle = session.StudyPlan?.Roadmap?.Title
                }
            };
        }
    }
}
