using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.DeleteSurveyTriggerType
{
    public class DeleteSurveyTriggerTypeHandler(IAppDbContext db)
        : IRequestHandler<DeleteSurveyTriggerTypeCommand, DeleteSurveyTriggerTypeResponse>
    {
        public async Task<DeleteSurveyTriggerTypeResponse> Handle(
            DeleteSurveyTriggerTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyTriggerTypes.FindAsync(new object?[] { request.Code }, cancellationToken);
                if (entity == null)
                {
                    return new DeleteSurveyTriggerTypeResponse(false, "SurveyTriggerType not found.");
                }

                db.SurveyTriggerTypes.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);
                return new DeleteSurveyTriggerTypeResponse(true, "SurveyTriggerType deleted successfully.");
            }
            catch (Exception ex)
            {
                return new DeleteSurveyTriggerTypeResponse(false, $"Error deleting SurveyTriggerType: {ex.Message}");
            }
        }
    }
}
