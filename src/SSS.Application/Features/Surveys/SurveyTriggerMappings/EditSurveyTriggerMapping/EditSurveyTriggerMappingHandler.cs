using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.EditSurveyTriggerMapping
{
    public class EditSurveyTriggerMappingHandler(IAppDbContext db): IRequestHandler<EditSurveyTriggerMappingCommand, EditSurveyTriggerMappingResponse>
    {
        public async Task<EditSurveyTriggerMappingResponse> Handle(EditSurveyTriggerMappingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyTriggerMappings.FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new EditSurveyTriggerMappingResponse(false, $"Survey Trigger Mapping with ID {request.Id} not found");
                }
                entity.SurveyId = request.SurveyId;
                entity.TriggerType = request.TriggerType;
                entity.MaxAttempts = request.MaxAttempts;
                entity.CooldownDays = request.CooldownDays;
                entity.IsActive = request.IsActive;
                entity.CreatedAt = request.CreatedAt;
                await db.SaveChangesAsync(cancellationToken);
                return new EditSurveyTriggerMappingResponse(true, "Survey Trigger Mapping updated successfully");
            }
            catch (Exception ex)
            {
                return new EditSurveyTriggerMappingResponse(false, $"Error while updating Survey Trigger Mapping: {ex.Message}");
            }
        }
    }
}
