using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.SurveyQuestions.EditSurveyQuestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.EditSurvey
{
    public class EditSurveyHandler(IAppDbContext db) : IRequestHandler<EditSurveyCommand, EditSurveyResponse>
    {
        public async Task<EditSurveyResponse> Handle(EditSurveyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.Surveys.FindAsync(new object?[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new EditSurveyResponse(false, "Survey not found.");
                }


                entity.Code = request.Code;
                entity.Title = request.Title;
                entity.Status = request.Status;
                
                await db.SaveChangesAsync(cancellationToken);
                return new EditSurveyResponse(true, "Survey updated successfully.");
            }
            catch (Exception ex)
            {
                return new EditSurveyResponse(false, $"Error editing survey: {ex.Message}");
            }
        }
    }
}
