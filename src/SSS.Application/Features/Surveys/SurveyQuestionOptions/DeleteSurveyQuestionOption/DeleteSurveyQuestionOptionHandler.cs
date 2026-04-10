using MediatR;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.DeleteSurveyQuestionOption
{
    public class DeleteSurveyQuestionOptionHandler(IAppDbContext db, ICacheService cacheService) : IRequestHandler<DeleteSurveyQuestionOptionCommand, DeleteSurveyQuestionOptionResponse>
    {
        public async Task<DeleteSurveyQuestionOptionResponse> Handle(DeleteSurveyQuestionOptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await db.SurveyQuestionOptions.FindAsync(new object?[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return new DeleteSurveyQuestionOptionResponse(false, "Not Found");
                }

                var questionId = entity.QuestionId;

                db.SurveyQuestionOptions.Remove(entity);
                await db.SaveChangesAsync(cancellationToken);

                var cacheKey = $"survey:question-options:{questionId}";
                await cacheService.RemoveAsync(cacheKey);

                return new DeleteSurveyQuestionOptionResponse(true, "Delete Successfully");
            }
            catch (Exception ex)
            {
                return new DeleteSurveyQuestionOptionResponse(false, $"Error while deleting: {ex.Message}");

            }
        }
    }
}
