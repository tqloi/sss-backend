using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.DeleteSurveyQuestion
{
    public class DeleteSurveyQuestionHandler(IAppDbContext db) : IRequestHandler<DeleteSurveyQuestionCommand, DeleteSurveyQuestionResponse>
    {
        public async Task<DeleteSurveyQuestionResponse> Handle(DeleteSurveyQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyQuestions.FindAsync(new object?[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new DeleteSurveyQuestionResponse(false, "Not Found");
                }
                db.SurveyQuestions.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);
                return new DeleteSurveyQuestionResponse(true, "Delete Successfully");
            }
            catch (Exception ex)
            {
                return new DeleteSurveyQuestionResponse(false, $"Error while deleting: {ex.Message}");

            }
        }
    }
}
