using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.StudyPlans.Common;

namespace SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanById
{
    public class GetStudyPlanByIdHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<GetStudyPlanByIdQuery, GetStudyPlanByIdResult>
    {
        public async Task<GetStudyPlanByIdResult> Handle(GetStudyPlanByIdQuery req, CancellationToken ct)
        {
            var studyPlan = await context.StudyPlans
                .AsNoTracking()
                .Where(sp => sp.Id == req.StudyPlanId)
                .ProjectTo<StudyPlanDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (studyPlan == null)
            {
                throw new NotFoundException($"Study plan with Id {req.StudyPlanId} not found");
            }

            return new GetStudyPlanByIdResult
            {
                Success = true,
                Data = studyPlan
            };
        }
    }
}
