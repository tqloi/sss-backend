using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.StudyPlans.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanByUser
{
    public class GetStudyPlanByUserHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<GetStudyPlanByUserQuery, GetStudyPlanByUserResult>
    {
        public async Task<GetStudyPlanByUserResult> Handle(GetStudyPlanByUserQuery req, CancellationToken ct)
        {
            var studyPlans = await context.StudyPlans
                .AsNoTracking()
                .Where(sp => sp.UserId == req.UserId)
                .Include(sp => sp.Roadmap)
                .ProjectTo<StudyPlanSummaryDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new GetStudyPlanByUserResult
            {
                Success = true,
                Data = studyPlans
            };
        }
    }
}
