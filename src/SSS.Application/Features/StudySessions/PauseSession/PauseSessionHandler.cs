using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.PauseSession
{
    public class PauseSessionHandler(IAppDbContext context)
        : IRequestHandler<PauseSessionCommand, PauseSessionResult>
    {
        public async Task<PauseSessionResult> Handle(PauseSessionCommand req, CancellationToken ct)
        {
            var session = await context.StudySessions
                .FirstOrDefaultAsync(s => s.Id == req.SessionId && s.UserId == req.UserId, ct)
                ?? throw new NotFoundException($"Session {req.SessionId} not found");

            if (session.Status != SessionStatus.InProgress)
                throw new ConflictException("Only InProgress sessions can be paused.");

            session.Status = SessionStatus.Paused;
            session.PauseCount += 1;
            session.PausedAt = DateTime.UtcNow;

            // Calculate active seconds so far (total elapsed - total pause time)
            var totalElapsed = (int)(DateTime.UtcNow - session.StartAt).TotalSeconds;

            await context.SaveChangesAsync(ct);

            return new PauseSessionResult
            {
                Success = true,
                Message = "Session paused",
                Data = new PauseSessionResponse
                {
                    SessionId = session.Id,
                    Status = session.Status.ToString(),
                    PauseCount = session.PauseCount,
                    PauseSeconds = session.PauseSeconds
                }
            };
        }
    }
}
