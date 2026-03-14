using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.AI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiAddBehaviorDb
{
    public class CreateAiAddBehaviorDbHandler(IPipeLine pipeLine, IAppDbContext db, IMapper mapper)
        : IRequestHandler<CreateAiAddBehaviorDbCommand, CreateAiAddBehaviorDbResult>
    {
        public async Task<CreateAiAddBehaviorDbResult> Handle(CreateAiAddBehaviorDbCommand req, CancellationToken ct)
        {
            var behavior = await db.UserLearningBehaviors
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.UserId == req.UserId, ct);

            var behaviorDto = mapper.Map<UserLearningBehaviorDto>(behavior);

            var result = await pipeLine.GenerateBehaviorResultAsync(behaviorDto, ct);

            if (result is null)
            {
                throw new Exception("Failed to generate behavior result.");
            }

            var chunks = new List<(string Text, string? Source)>
            {
                (result, "user_behavior")
            };

            await pipeLine.IngestBehaviorAsync(req.StudyPlanId, req.UserId, chunks, ct);

            return new CreateAiAddBehaviorDbResult
            {
                Success = true,
                Message = "Behavior added to the database successfully."
            };
        }
    }
}
