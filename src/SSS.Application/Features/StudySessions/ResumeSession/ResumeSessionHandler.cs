using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.ResumeSession
{
    public class ResumeSessionHandler(IAppDbContext context)
        : IRequestHandler<ResumeSessionCommand, ResumeSessionResult>
    {
        public async Task<ResumeSessionResult> Handle(ResumeSessionCommand req, CancellationToken ct)
        {
            var session = await context.StudySessions
                .FirstOrDefaultAsync(s => s.Id == req.SessionId && s.UserId == req.UserId, ct)
                ?? throw new NotFoundException($"Session {req.SessionId} not found");

            if (session.Status != SessionStatus.Paused)
                throw new ConflictException("Only Paused sessions can be resumed.");

            // Accumulate pause time
            if (session.PausedAt.HasValue)
            {
                session.PauseSeconds += (int)(DateTime.UtcNow - session.PausedAt.Value).TotalSeconds;
            }

            session.Status = SessionStatus.InProgress;
            session.PausedAt = null;

            await context.SaveChangesAsync(ct);

            return new ResumeSessionResult
            {
                Success = true,
                Message = "Session resumed",
                Data = new ResumeSessionResponse
                {
                    SessionId = session.Id,
                    Status = session.Status.ToString()
                }
            };
        }
    }
}
