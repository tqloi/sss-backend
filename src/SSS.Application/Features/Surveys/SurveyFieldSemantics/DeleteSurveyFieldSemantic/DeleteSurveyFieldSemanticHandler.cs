using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic
{
    public class DeleteSurveyFieldSemanticHandler(IAppDbContext db): IRequestHandler<DeleteSurveyFieldSemanticCommand, DeleteSurveyFieldSemanticResponse>
    {
        public async Task<DeleteSurveyFieldSemanticResponse> Handle(DeleteSurveyFieldSemanticCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyFieldSemantics
                    .FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new DeleteSurveyFieldSemanticResponse(false, "Survey Field Semantic not found");
                }
                db.SurveyFieldSemantics.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);
                return new DeleteSurveyFieldSemanticResponse(true, "Survey Field Semantic deleted successfully");
            }
            catch (Exception ex)
            {
                return new DeleteSurveyFieldSemanticResponse(false, $"Error while deleting Survey Field Semantic: {ex.Message}");
            }
        }
    
    }
}
