using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SurveyTriggerTypeCodes = SSS.Domain.Constants.SurveyTriggerTypes;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.GetPendingTriggerSurvey
{
    public class GetPendingTriggerSurveyHandler(IAppDbContext db)
        : IRequestHandler<GetPendingTriggerSurveyQuery, GetPendingTriggerSurveyResult>
    {
        public async Task<GetPendingTriggerSurveyResult> Handle(
            GetPendingTriggerSurveyQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Find the single active mapping for this TriggerType
            //    (business rule: at most one active mapping per TriggerType, enforced by Create/Edit)
            var mapping = await db.SurveyTriggerMappings
                .AsNoTracking()
                .Include(m => m.Survey)
                .Where(m => m.TriggerType == request.TriggerType && m.IsActive)
                .FirstOrDefaultAsync(cancellationToken);

            if (mapping is null)
            {
                return new GetPendingTriggerSurveyResult
                {
                    HasPendingSurvey = false,
                    TriggerType = request.TriggerType
                };
            }

            var now = DateTime.UtcNow;

            // 2. Get all completed responses for this user + this survey
            var completedResponses = await db.SurveyResponses
                .AsNoTracking()
                .Where(r =>
                    r.UserId == request.UserId &&
                    r.SurveyId == mapping.SurveyId &&
                    r.SubmittedAt != null)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync(cancellationToken);

            var completedCount = completedResponses.Count;

            // ON_REGISTER can only be taken once.
            if (request.TriggerType == SurveyTriggerTypeCodes.OnRegister && completedCount > 0)
            {
                return new GetPendingTriggerSurveyResult
                {
                    HasPendingSurvey = false,
                    TriggerType = request.TriggerType,
                    CompletedAttempts = completedCount,
                    BlockedReason = "AlreadyCompletedOnRegister"
                };
            }

            // 2a. Check MaxAttempts — not eligible if exhausted
            if (mapping.MaxAttempts.HasValue && completedCount >= mapping.MaxAttempts.Value)
            {
                return new GetPendingTriggerSurveyResult
                {
                    HasPendingSurvey = false,
                    TriggerType = request.TriggerType,
                    CompletedAttempts = completedCount,
                    MaxAttempts = mapping.MaxAttempts,
                    BlockedReason = "MaxAttemptsExceeded"
                };
            }

            // 2b. Check CooldownDays — not eligible if still within cooldown window
            if (mapping.CooldownDays.HasValue && completedCount > 0)
            {
                var lastCompleted = completedResponses[0].SubmittedAt!.Value;
                var cooldownEnd = lastCompleted.AddDays(mapping.CooldownDays.Value);
                if (now < cooldownEnd)
                {
                    return new GetPendingTriggerSurveyResult
                    {
                        HasPendingSurvey = false,
                        TriggerType = request.TriggerType,
                        CompletedAttempts = completedCount,
                        CooldownDays = mapping.CooldownDays,
                        BlockedReason = "CooldownActive",
                        CooldownEndsAt = cooldownEnd
                    };
                }
            }

            // 2c. Mapping is eligible — return it
            return new GetPendingTriggerSurveyResult
            {
                HasPendingSurvey = true,
                SurveyId = mapping.SurveyId,
                SurveyCode = mapping.Survey.Code,
                SurveyTitle = mapping.Survey.Title,
                TriggerType = request.TriggerType,
                CompletedAttempts = completedCount,
                MaxAttempts = mapping.MaxAttempts,
                CooldownDays = mapping.CooldownDays
            };
        }
    }
}
