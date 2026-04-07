using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.StudyPlans.Common;
using SSS.Domain.Constants;

namespace SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanById
{
    public class GetStudyPlanByIdHandler(
        IAppDbContext context,
        IMapper mapper,
        ICacheService cacheService
        ) : IRequestHandler<GetStudyPlanByIdQuery, GetStudyPlanByIdResult>
    {
        public async Task<GetStudyPlanByIdResult> Handle(GetStudyPlanByIdQuery req, CancellationToken ct)
        {
            var cacheKey = $"studyplan:id:{req.StudyPlanId}";

            var cachedStudyPlan = await cacheService.GetAsync<StudyPlanDto>(cacheKey);
            if (cachedStudyPlan != null)
            {
                return new GetStudyPlanByIdResult
                {
                    Success = true,
                    Data = cachedStudyPlan
                };
            }

            var studyPlan = await context.StudyPlans
                .AsNoTracking()
                .Where(sp => sp.Id == req.StudyPlanId)
                .ProjectTo<StudyPlanDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (studyPlan == null)
            {
                throw new NotFoundException($"Study plan with Id {req.StudyPlanId} not found");
            }

            await cacheService.SetAsync(cacheKey, studyPlan, CacheConstants.DefaultExpiration);

            return new GetStudyPlanByIdResult
            {
                Success = true,
                Data = studyPlan
            };
        }
    }
}
