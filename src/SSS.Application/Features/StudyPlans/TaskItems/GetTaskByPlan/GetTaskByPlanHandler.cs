using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.StudyPlans.TaskItems.Common;

namespace SSS.Application.Features.StudyPlans.TaskItems.GetTaskByPlan
{
    public class GetTaskByPlanHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<GetTaskByPlanQuery, GetTaskByPlanResult>
    {
        public async Task<GetTaskByPlanResult> Handle(GetTaskByPlanQuery req, CancellationToken ct)
        {
            var tasks = await context.TaskItems
                .AsNoTracking()
                .Where(t => t.StudyPlanModule.StudyPlanId == req.StudyPlanId)
                .ProjectTo<TaskItemDtos>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new GetTaskByPlanResult
            {
                Success = true,
                Message = $"Retrieved {tasks.Count} tasks",
                Data = tasks
            };
        }
    }
}
