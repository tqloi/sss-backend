using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.TaskItems.Common;
using SSS.Domain.Entities.Planning;

namespace SSS.Application.Features.StudyPlans.TaskItems.CreateTaskList
{
    public class CreateTaskListHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<CreateTaskListCommand, CreateTaskListResult>
    {
        public async Task<CreateTaskListResult> Handle(CreateTaskListCommand req, CancellationToken ct)
        {
            // Verify all StudyPlanModules exist
            var moduleIds = req.Tasks.Select(t => t.StudyPlanModuleId).Distinct().ToList();
            var existingModuleIds = await context.StudyPlanModules
                .Where(m => moduleIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync(ct);

            var missingModuleIds = moduleIds.Except(existingModuleIds).ToList();
            if (missingModuleIds.Any())
            {
                throw new NotFoundException($"Study plan modules with Ids {string.Join(", ", missingModuleIds)} not found");
            }

            var taskItems = req.Tasks.Select(t => new TaskItem
            {
                StudyPlanModuleId = t.StudyPlanModuleId,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                EstimatedDurationSeconds = t.EstimatedDurationSeconds,
                ScheduledDate = t.ScheduledDate
            }).ToList();

            context.TaskItems.AddRange(taskItems);
            await context.SaveChangesAsync(ct);

            var taskIds = taskItems.Select(t => t.Id).ToList();
            var dtos = await context.TaskItems
                .AsNoTracking()
                .Where(t => taskIds.Contains(t.Id))
                .ProjectTo<TaskItemDtos>(mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return new CreateTaskListResult
            {
                Success = true,
                Message = $"{dtos.Count} tasks created successfully",
                Data = dtos
            };
        }
    }
}
