using MediatR;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.SurveyQuestions.EditSurveyQuestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.EditSurvey
{
    public class EditSurveyHandler(IAppDbContext db, ICacheService cacheService) : IRequestHandler<EditSurveyCommand, EditSurveyResponse>
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

                var oldCode = entity.Code;


                entity.Code = request.Code;
                entity.Title = request.Title;
                entity.Status = request.Status;
                
                await db.SaveChangesAsync(cancellationToken);

                var oldCacheKey = BuildSurveyCodeCacheKey(oldCode);
                if (oldCacheKey != null)
                {
                    await cacheService.RemoveAsync(oldCacheKey);
                }

                var newCacheKey = BuildSurveyCodeCacheKey(request.Code);
                if (newCacheKey != null && !string.Equals(newCacheKey, oldCacheKey, StringComparison.Ordinal))
                {
                    await cacheService.RemoveAsync(newCacheKey);
                }

                return new EditSurveyResponse(true, "Survey updated successfully.");
            }
            catch (Exception ex)
            {
                return new EditSurveyResponse(false, $"Error editing survey: {ex.Message}");
            }
        }

        private static string? BuildSurveyCodeCacheKey(string? surveyCode)
        {
            if (string.IsNullOrWhiteSpace(surveyCode))
            {
                return null;
            }

            return $"survey:code:{surveyCode.Trim().ToLowerInvariant()}";
        }
    }
}
