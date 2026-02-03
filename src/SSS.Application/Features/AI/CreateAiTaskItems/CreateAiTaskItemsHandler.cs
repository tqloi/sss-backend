using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.AI.CreateAiRoadMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiTaskItems
{
    public sealed class CreateAiTaskItemsHandler(IPipeLine pipeLine, IAppDbContext dbContext) 
        : IRequestHandler<CreateAiTaskItemsCommand, CreateAiTaskItemsResult>
    {
        public async Task<CreateAiTaskItemsResult> Handle(CreateAiTaskItemsCommand request, CancellationToken cancellationToken)
        {
            
            var module = await dbContext.StudyPlanModules
                .AsNoTracking()
                .FirstOrDefaultAsync(spm => spm.Id == request.studyPlanModuleId);

            var roadmapnode = await dbContext.RoadmapNodes
                .AsNoTracking()
                .FirstOrDefaultAsync(rm => rm.Id == module.RoadmapNodeId);

            var roadmap = await dbContext.Roadmaps
                .AsNoTracking()
                .FirstOrDefaultAsync(rm => rm.Id == roadmapnode.RoadmapId);

            var aiResponse = await pipeLine.GenerateStudyPlanAsync(request.UserId, roadmap, roadmapnode, cancellationToken);

            if (aiResponse == null) throw new NotImplementedException();

            aiResponse = aiResponse
           .Replace("```json", "")
           .Replace("```", "")
           .Trim();

            using var doc = JsonDocument.Parse(aiResponse);

            return new CreateAiTaskItemsResult
            {
                Success = true,
                Message = "AI task generated successfully",
                RawTaskItens = doc.RootElement.Clone()
            };
        }
    }
}
