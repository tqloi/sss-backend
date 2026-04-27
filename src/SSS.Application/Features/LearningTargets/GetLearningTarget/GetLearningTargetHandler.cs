using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Enums;

namespace SSS.Application.Features.LearningTargets.GetLearningTarget
{
    public class GetLearningTargetHandler : IRequestHandler<GetLearningTargetQuery, GetLearningTargetResult?>
    {
        private readonly IAppDbContext _dbContext;

        public GetLearningTargetHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetLearningTargetResult?> Handle(GetLearningTargetQuery request, CancellationToken cancellationToken)
        {
            var target = await _dbContext.UserLearningTargets
                .Where(x => x.UserId == request.UserId && x.RoadmapId == request.RoadmapId && x.Status == TargetStatus.active)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new GetLearningTargetResult
                {
                    TargetRole = x.TargetRole,
                    CurrentLevel = x.CurrentLevel,
                    TargetDeadlineMonths = x.TargetDeadlineMonths
                })
                .FirstOrDefaultAsync(cancellationToken);

            return target;
        }
    }
}
