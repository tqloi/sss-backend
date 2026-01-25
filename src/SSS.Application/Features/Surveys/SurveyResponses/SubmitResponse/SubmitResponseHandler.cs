using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.SurveyQuestions.EditSurveyQuestion;
using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.SubmitResponse
{
    public class SubmitResponseHandler(IAppDbContext db) : IRequestHandler<SubmitResponseCommand, SubmitResponseResponse>
    {
        public async Task<SubmitResponseResponse> Handle(SubmitResponseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyResponses.FindAsync(new object?[] { request.ResponseId }, cancellationToken);
                if (entity == null)
                {
                    return new SubmitResponseResponse(false, "Response not found.");
                }

                // Lấy tất cả các answers có ResponseId tương ứng
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

                // Serialize answers thành JSON
                var snapshotJson = JsonSerializer.Serialize(answers, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                entity.SurveyId = request.SurveyId;
                entity.UserId = request.UserId;
                entity.SubmittedAt = request.SubmittedAt;
                entity.SnapshotJson = snapshotJson;
                entity.TriggerReason = request.TriggerReason;

                await db.SaveChangesAsync(cancellationToken);
                return new SubmitResponseResponse(true, "Response submitted successfully.");
            }
            catch (Exception ex)
            {
                return new SubmitResponseResponse(false, $"Error submitting response: {ex.Message}");
            }
        }
    }
}
