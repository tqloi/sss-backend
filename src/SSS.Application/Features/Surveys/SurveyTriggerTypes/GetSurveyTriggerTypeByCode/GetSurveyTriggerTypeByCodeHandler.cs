using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.GetSurveyTriggerTypeByCode
{
    public class GetSurveyTriggerTypeByCodeHandler(IAppDbContext db)
        : IRequestHandler<GetSurveyTriggerTypeByCodeQuery, GetSurveyTriggerTypeByCodeResult>
    {
        public async Task<GetSurveyTriggerTypeByCodeResult> Handle(
            GetSurveyTriggerTypeByCodeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyTriggerTypes.FindAsync(new object?[] { request.Code }, cancellationToken);
                if (entity == null)
                {
                    return new GetSurveyTriggerTypeByCodeResult(false, "SurveyTriggerType not found.");
                }

                var dto = new SurveyTriggerTypeDto(
                    entity.Code,
                    entity.DisplayName,
                    entity.Description,
                    entity.IsActive
                );
                return new GetSurveyTriggerTypeByCodeResult(true, "Success", dto);
            }
            catch (Exception ex)
            {
                return new GetSurveyTriggerTypeByCodeResult(false, $"Error retrieving SurveyTriggerType: {ex.Message}");
            }
        }
    }
}
