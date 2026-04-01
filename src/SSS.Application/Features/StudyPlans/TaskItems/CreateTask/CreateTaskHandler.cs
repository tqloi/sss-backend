using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.TaskItems.Common;
using SSS.Domain.Entities.Planning;

namespace SSS.Application.Features.StudyPlans.TaskItems.CreateTask
{
    public class CreateTaskHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<CreateTaskCommand, CreateTaskResult>
    {
        public async Task<CreateTaskResult> Handle(CreateTaskCommand req, CancellationToken ct)
        {
            // Verify StudyPlanModule exists
            if (!await context.StudyPlanModules.AnyAsync(m => m.Id == req.StudyPlanModuleId, ct))
            {
                throw new NotFoundException($"Study plan module with Id {req.StudyPlanModuleId} not found");
            }

            var taskItem = new TaskItem
            {
                StudyPlanModuleId = req.StudyPlanModuleId,
                Title = req.Title,
                Description = req.Description,
                Status = req.Status,
                EstimatedDurationSeconds = req.EstimatedDurationSeconds,
                ScheduledDate = req.ScheduledDate
            };

            context.TaskItems.Add(taskItem);
            await context.SaveChangesAsync(ct);

            var dto = await context.TaskItems
                .AsNoTracking()
                .Where(t => t.Id == taskItem.Id)
                .ProjectTo<TaskItemDtos>(mapper.ConfigurationProvider)
                .FirstAsync(ct);

            return new CreateTaskResult
            {
                Success = true,
                Message = "Task created successfully",
                Data = dto
            };
        }
    }
}
