using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.StudyPlans.Common;

namespace SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanByRoamapId
{
    public class GetStudyPlanByRoadmapIdHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<GetStudyPlanByRoadmapIdQuery, GetStudyPlanByRoadmapIdResult>
    {
        public async Task<GetStudyPlanByRoadmapIdResult> Handle(GetStudyPlanByRoadmapIdQuery req, CancellationToken ct)
        {
            var studyPlan = await context.StudyPlans
                .AsNoTracking()
                .Where(sp => sp.UserId == req.UserId && sp.RoadmapId == req.RoadmapId)
                .ProjectTo<StudyPlanDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (studyPlan == null)
            {
                throw new NotFoundException($"Study plan not found for user {req.UserId} and roadmap {req.RoadmapId}");
            }

            return new GetStudyPlanByRoadmapIdResult
            {
                Success = true,
                Data = studyPlan
            };
        }
    }
}
