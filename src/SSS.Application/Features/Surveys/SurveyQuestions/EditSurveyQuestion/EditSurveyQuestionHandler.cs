using MediatR;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.EditSurveyQuestion
{
    public class EditSurveyQuestionHandler(IAppDbContext db, ICacheService cacheService) : IRequestHandler<EditSurveyQuestionCommand, EditSurveyQuestionResponse>
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

                var oldSurveyId = entity.SurveyId;


                entity.SurveyId = request.SurveyId;
                entity.QuestionKey = request.QuestionKey;
                entity.Prompt = request.Prompt;
                entity.IsRequired = request.IsRequired;
                entity.OrderNo = request.OrderNo;
                entity.Type = request.Type;
                entity.ScaleMax = request.ScaleMax;
                entity.ScaleMin = request.ScaleMin;
                await db.SaveChangesAsync(cancellationToken);

                await cacheService.RemoveAsync($"survey:questions:{oldSurveyId}");
                if (oldSurveyId != request.SurveyId)
                {
                    await cacheService.RemoveAsync($"survey:questions:{request.SurveyId}");
                }

                return new EditSurveyQuestionResponse(true, "Question updated successfully.");
            }
            catch (Exception ex)
            {
                return new EditSurveyQuestionResponse(false, $"Error editing question: {ex.Message}");
            }
        }
    }
}
