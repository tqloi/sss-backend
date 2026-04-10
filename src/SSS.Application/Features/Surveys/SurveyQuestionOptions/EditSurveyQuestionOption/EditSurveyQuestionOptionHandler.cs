using MediatR;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.EditSurveyQuestionOption
{
    public class EditSurveyQuestionOptionHandler(IAppDbContext db, ICacheService cacheService) : IRequestHandler<EditSurveyQuestionOptionCommand, EditSurveyQuestionOptionResponse>
    {
        public async Task<EditSurveyQuestionOptionResponse> Handle(EditSurveyQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyQuestionOptions.FindAsync(new object?[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new EditSurveyQuestionOptionResponse(false, "Question option not found.");
                }

                var oldQuestionId = entity.QuestionId;

                entity.QuestionId = request.QuestionId;
                entity.ValueKey = request.ValueKey;
                entity.DisplayText = request.DisplayText;
                entity.Weight = request.Weight;
                entity.OrderNo = request.OrderNo;
                entity.AllowFreeText = request.AllowFreeText;
                
                await db.SaveChangesAsync(cancellationToken);

                await cacheService.RemoveAsync($"survey:question-options:{oldQuestionId}");
                if (oldQuestionId != request.QuestionId)
                {
                    await cacheService.RemoveAsync($"survey:question-options:{request.QuestionId}");
                }

                return new EditSurveyQuestionOptionResponse(true, "Question Option updated successfully.");
            }
            catch (Exception ex)
            {
                return new EditSurveyQuestionOptionResponse(false, $"Error editing question option: {ex.Message}");
            }
        }
    }
}
