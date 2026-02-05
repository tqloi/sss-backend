using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.AI.Common;
using SSS.Application.Features.AI.CreateAiTaskItems;
using SSS.Domain.Entities.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiAddVecDb
{
    public sealed class CreateAiAddVecDbHandler(IPipeLine pipeLine, IAppDbContext dbContext, IMapper mapper)
        : IRequestHandler<CreateAiAddVecDbCommand, CreateAiAddVecDbResponse>
    {
        public async Task<CreateAiAddVecDbResponse> Handle(CreateAiAddVecDbCommand request, CancellationToken cancellationToken)
        {
            var behavior = await dbContext.UserLearningBehaviors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            var target = await dbContext.UserLearningTargets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            var behaviorDto = mapper.Map<UserLearningBehaviorDto>(behavior);
            var targetDto = mapper.Map<UserLearningTargetDto>(target);

            var surveyResult = await pipeLine.GenerateSurveyResultAsync(targetDto, behaviorDto, cancellationToken);

            var chunks = new List<(string Text, string? Source)>
            {
                (surveyResult, "user_profile")
            };
            if (surveyResult is null)
            {
                throw new Exception("Failed to generate survey result.");
            }
           await pipeLine.IngestAsync(request.UserId, chunks, cancellationToken);
            return new CreateAiAddVecDbResponse
            {
                Message = "Vector database updated successfully.",
                 Success = true
            };
        }    
    }
}
