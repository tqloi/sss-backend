using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.Roadmap.Create
{
    public sealed class CreateRoadmapHandler(IAppDbContext dbContext) 
        : IRequestHandler<CreateRoadmapCommand, CreateRoadmapResult>
    {
        public async Task<CreateRoadmapResult> Handle(CreateRoadmapCommand request, CancellationToken cancellationToken)
        {
            // Validate SubjectId exists
            var subjectExists = await dbContext.LearningSubjects
                .AnyAsync(x => x.Id == request.SubjectId, cancellationToken);

            if (!subjectExists)
            {
                return new CreateRoadmapResult
                {
                    Success = false,
                    Message = "Learning subject not found.",
                    Data = null
                };
            }

            var entity = new Domain.Entities.Content.Roadmap
            {
                SubjectId = request.SubjectId,
                Title = request.Title,
                Description = request.Description
            };

            dbContext.Roadmaps.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateRoadmapResult
            {
                Success = true,
                Message = "Roadmap created successfully.",
                Data = new RoadmapBasicDTO
                {
                    Id = entity.Id,
                    SubjectId = entity.SubjectId,
                    Title = entity.Title,
                    Description = entity.Description
                }
            };
        }
    }
}
