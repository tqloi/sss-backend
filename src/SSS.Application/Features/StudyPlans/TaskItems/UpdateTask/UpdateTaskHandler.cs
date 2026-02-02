using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.TaskItems.Common;

namespace SSS.Application.Features.StudyPlans.TaskItems.UpdateTask
{
    public class UpdateTaskHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<UpdateTaskCommand, UpdateTaskResult>
    {
        public async Task<UpdateTaskResult> Handle(UpdateTaskCommand req, CancellationToken ct)
        {
            var taskItem = await context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == req.TaskId, ct);

            if (taskItem == null)
            {
                throw new NotFoundException($"Task with Id {req.TaskId} not found");
            }

            if (req.Title != null)
                taskItem.Title = req.Title;

            if (req.Title != null)
                taskItem.Description = req.Description;

            if (req.Status.HasValue)
                taskItem.Status = req.Status.Value;

            if (req.EstimatedDurationSeconds.HasValue)
                taskItem.EstimatedDurationSeconds = req.EstimatedDurationSeconds.Value;

            if (req.ScheduledDate.HasValue)
                taskItem.ScheduledDate = req.ScheduledDate.Value;

            if (req.CompletedAt.HasValue)
                taskItem.CompletedAt = req.CompletedAt.Value;

            await context.SaveChangesAsync(ct);

            var dto = await context.TaskItems
                .AsNoTracking()
                .Where(t => t.Id == taskItem.Id)
                .ProjectTo<TaskItemDtos>(mapper.ConfigurationProvider)
                .FirstAsync(ct);

            return new UpdateTaskResult
            {
                Success = true,
                Message = "Task updated successfully",
                Data = dto
            };
        }
    }
}
