using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;

namespace SSS.Application.Features.StudyPlans.TaskItems.DeleteTask
{
    public class DeleteTaskHandler(
        IAppDbContext context
        ) : IRequestHandler<DeleteTaskCommand, DeleteTaskResult>
    {
        public async Task<DeleteTaskResult> Handle(DeleteTaskCommand req, CancellationToken ct)
        {
            var taskItem = await context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == req.TaskId, ct);

            if (taskItem == null)
            {
                throw new NotFoundException($"Task with Id {req.TaskId} not found");
            }

            context.TaskItems.Remove(taskItem);
            await context.SaveChangesAsync(ct);

            return new DeleteTaskResult
            {
                Success = true,
                Message = "Task deleted successfully",
                Data = null
            };
        }
    }
}
