using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.EditSurveyTriggerType
{
    public class EditSurveyTriggerTypeHandler(IAppDbContext db)
        : IRequestHandler<EditSurveyTriggerTypeCommand, EditSurveyTriggerTypeResponse>
    {
        public async Task<EditSurveyTriggerTypeResponse> Handle(
            EditSurveyTriggerTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyTriggerTypes.FindAsync(new object?[] { request.Code }, cancellationToken);
                if (entity == null)
                {
                    return new EditSurveyTriggerTypeResponse(false, "SurveyTriggerType not found.");
                }

                entity.DisplayName = request.DisplayName;
                entity.Description = request.Description;
                entity.IsActive = request.IsActive;

                await db.SaveChangesAsync(cancellationToken);
                return new EditSurveyTriggerTypeResponse(true, "SurveyTriggerType updated successfully.");
            }
            catch (Exception ex)
            {
                return new EditSurveyTriggerTypeResponse(false, $"Error updating SurveyTriggerType: {ex.Message}");
            }
        }
    }
}
