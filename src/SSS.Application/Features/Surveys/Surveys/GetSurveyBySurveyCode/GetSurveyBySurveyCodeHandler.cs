using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Constants;

namespace SSS.Application.Features.Surveys.Surveys.GetSurveyBySurveyCode;

public class GetSurveyBySurveyCodeHandler(IAppDbContext db, IMapper mapper, ICacheService cacheService) 
    : IRequestHandler<GetSurveyBySurveyCodeQuery, GetSurveyBySurveyCodeResult>
{
    public async Task<GetSurveyBySurveyCodeResult> Handle(
        GetSurveyBySurveyCodeQuery request, 
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.SurveyCode))
            {
                return new GetSurveyBySurveyCodeResult(false, "Survey code is required");
            }

            var normalizedSurveyCode = request.SurveyCode.Trim();
            var cacheKey = $"survey:code:{normalizedSurveyCode.ToLowerInvariant()}";

            var cachedSurvey = await cacheService.GetAsync<SurveyDto>(cacheKey);
            if (cachedSurvey != null)
            {
                return new GetSurveyBySurveyCodeResult(true, "Survey retrieved successfully", cachedSurvey);
            }

            var survey = await db.Surveys
                .AsNoTracking()
                .Include(s => s.Questions.OrderBy(q => q.OrderNo))
                    .ThenInclude(q => q.Options.OrderBy(o => o.OrderNo))
                .FirstOrDefaultAsync(s => s.Code == normalizedSurveyCode, cancellationToken);

            if (survey == null)
            {
                return new GetSurveyBySurveyCodeResult(false, $"Survey with code '{normalizedSurveyCode}' not found");
            }

            var dto = mapper.Map<SurveyDto>(survey);
            await cacheService.SetAsync(cacheKey, dto, CacheConstants.DefaultExpiration);

            return new GetSurveyBySurveyCodeResult(true, "Survey retrieved successfully", dto);
        }
        catch (Exception ex)
        {
            return new GetSurveyBySurveyCodeResult(false, $"Error retrieving survey: {ex.Message}");
        }
    }
}