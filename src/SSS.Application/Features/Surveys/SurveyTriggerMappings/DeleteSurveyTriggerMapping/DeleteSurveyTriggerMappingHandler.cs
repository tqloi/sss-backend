using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.DeleteSurveyTriggerMapping
{
    public class DeleteSurveyTriggerMappingHandler(IAppDbContext db) : IRequestHandler<DeleteSurveyTriggerMappingCommand, DeleteSurveyTriggerMappingResponse>
    {
        public async Task<DeleteSurveyTriggerMappingResponse> Handle(DeleteSurveyTriggerMappingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyTriggerMappings
                    .FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new DeleteSurveyTriggerMappingResponse(false, "SurveyTriggerMapping not found");
                }
                db.SurveyTriggerMappings.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);
                return new DeleteSurveyTriggerMappingResponse(true, "SurveyTriggerMapping deleted successfully");
            }
            catch (Exception ex)
            {
                return new DeleteSurveyTriggerMappingResponse(false, $"Error while deleting SurveyTriggerMapping: {ex.Message}");
            }
        }
    }
}
