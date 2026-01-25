using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.EditSurveyQuestion
{
    public class EditSurveyQuestionHandler(IAppDbContext db) : IRequestHandler<EditSurveyQuestionCommand, EditSurveyQuestionResponse>
    {
        public async Task<EditSurveyQuestionResponse> Handle(EditSurveyQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyQuestions.FindAsync(new object?[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new EditSurveyQuestionResponse(false, "Question not found.");
                }


                entity.SurveyId = request.SurveyId;
                entity.QuestionKey = request.QuestionKey;
                entity.Prompt = request.Prompt;
                entity.IsRequired = request.IsRequired;
                entity.OrderNo = request.OrderNo;
                entity.Type = request.Type;
                entity.ScaleMax = request.ScaleMax;
                entity.ScaleMin = request.ScaleMin;
                await db.SaveChangesAsync(cancellationToken);
                return new EditSurveyQuestionResponse(true, "Question updated successfully.");
            }
            catch (Exception ex)
            {
                return new EditSurveyQuestionResponse(false, $"Error editing question: {ex.Message}");
            }
        }
    }
}
