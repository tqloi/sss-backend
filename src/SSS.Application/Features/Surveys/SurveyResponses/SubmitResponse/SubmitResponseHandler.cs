using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Background;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Constants;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Surveys.SurveyResponses.SubmitResponse
{
    public class SubmitResponseHandler(
        IAppDbContext db,
        ISurveyJobDispatcher jobDispatcher)
        : IRequestHandler<SubmitResponseCommand, SubmitResponseResponse>
    {
        public async Task<SubmitResponseResponse> Handle(SubmitResponseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyResponses.FindAsync(new object?[] { request.ResponseId }, cancellationToken);
                if (entity == null)
                    return new SubmitResponseResponse(false, "Response not found.");

                // Snapshot answers at submission time
                var answers = await db.SurveyAnswers
                    .Where(a => a.ResponseId == request.ResponseId)
                    .Select(a => new
                    {
                        a.Id,
                        a.ResponseId,
                        a.QuestionId,
                        a.OptionId,
                        a.NumberValue,
                        a.TextValue,
                        a.AnsweredAt
                    })
                    .ToListAsync(cancellationToken);

                entity.SurveyId     = request.SurveyId;
                entity.UserId       = request.UserId;
                entity.SubmittedAt  = request.SubmittedAt;
                entity.SnapshotJson = JsonSerializer.Serialize(answers, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                entity.TriggerReason = request.TriggerReason;

                await db.SaveChangesAsync(cancellationToken);

                // Dispatch the appropriate background job based on survey type
                var survey = await db.Surveys
                    .FirstOrDefaultAsync(s => s.Id == request.SurveyId, cancellationToken);

                if (survey != null)
                {
                    if (survey.Code == SurveyCodes.LearningBehavior)
                    {
                        jobDispatcher.DispatchBehaviorAnalysis(request.ResponseId);
                    }
                    else if (survey.Code == SurveyCodes.RoadmapLearningTarget)
                    {
                        if (!request.RoadmapId.HasValue)
                            return new SubmitResponseResponse(false,
                                "RoadmapId is required when submitting a ROADMAP_LEARNING_TARGET survey.");

                        jobDispatcher.DispatchTargetAnalysis(request.ResponseId, request.RoadmapId.Value);
                    }
                }

                return new SubmitResponseResponse(true, "Response submitted successfully.");
            }
            catch (Exception ex)
            {
                return new SubmitResponseResponse(false, $"Error submitting response: {ex.Message}");
            }
        }
    }
}
