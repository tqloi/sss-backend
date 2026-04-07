using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.TaskItems.Common;

namespace SSS.Application.Features.StudyPlans.TaskItems.GetTaskById
{
    public class GetTaskByIdHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<GetTaskByIdQuery, GetTaskByIdResult>
    {
        public async Task<GetTaskByIdResult> Handle(GetTaskByIdQuery req, CancellationToken ct)
        {
            var task = await context.TaskItems
                .AsNoTracking()
                .Where(t => t.Id == req.TaskId)
                .ProjectTo<TaskItemDtos>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            if (task == null)
            {
                throw new NotFoundException($"Task with Id {req.TaskId} not found");
            }

            return new GetTaskByIdResult
            {
                Success = true,
                Message = "Task retrieved successfully",
                Data = task
            };
        }
    }
}
