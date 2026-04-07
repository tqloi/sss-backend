using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.DeleteSurvey
{
    public class DeleteSurveyHandler(IAppDbContext db) : IRequestHandler<DeleteSurveyCommand, DeleteSurveyResponse>
    {
        public async Task<DeleteSurveyResponse> Handle(DeleteSurveyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.Surveys.FindAsync(new object?[] { request.Id }, cancellationToken);
                if (entity == null) 
                { 
                return new DeleteSurveyResponse(false, "Not Found"); 
                }
                db.Surveys.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);
                return new DeleteSurveyResponse(true, "Delete Successfully");
            }
            catch (Exception ex)
            {
                return new DeleteSurveyResponse(false, $"Error while deleting: {ex.Message}");

            }
        }
    }
}
