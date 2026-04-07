using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.StudyPlans.TaskItems.Common;

namespace SSS.Application.Features.StudyPlans.TaskItems.GetTaskByModule
{
    public class GetTaskByModuleHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<GetTaskByModuleQuery, GetTaskByModuleResult>
    {
        public async Task<GetTaskByModuleResult> Handle(GetTaskByModuleQuery req, CancellationToken ct)
        {
            var tasks = await context.TaskItems
                .AsNoTracking()
                .Where(t => t.StudyPlanModuleId == req.StudyPlanModuleId)
                .ProjectTo<TaskItemDtos>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new GetTaskByModuleResult
            {
                Success = true,
                Message = $"Retrieved {tasks.Count} tasks",
                Data = tasks
            };
        }
    }
}
