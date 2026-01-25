using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Surveys.DeleteSurvey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.DeleteResponse
{
    public class DeleteResponseHandler(IAppDbContext db) : IRequestHandler<DeleteResponseCommand, DeleteResponseResponse>
    {
        public async Task<DeleteResponseResponse> Handle(DeleteResponseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyResponses.FindAsync(new object?[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new DeleteResponseResponse(false, "Not Found");
                }
                db.SurveyResponses.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);
                return new DeleteResponseResponse(true, "Delete Successfully");
            }
            catch (Exception ex)
            {
                return new DeleteResponseResponse(false, $"Error while deleting: {ex.Message}");

            }
        }
    }
}
