using MediatR;
using SSS.Application.Abstractions.External.AiServices.LLM;
using SSS.Application.Abstractions.External.AiServices.PipeLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateRoadMap
{
    public sealed class CreateRoadMapHandler(IPipeLine pipeLine)
        : IRequestHandler<CreateRoadMapRequest, CreateRoadMapResponse>
    {
        public async Task<CreateRoadMapResponse> Handle(CreateRoadMapRequest request, CancellationToken cancellationToken)
        {
            var rawJson = await pipeLine.AskAsync(
                request.Message,
                topK: 5,
                ct: cancellationToken
            );
            // 2. Deserialize
            var response = new CreateRoadMapResponse(rawJson);
            if (response is null)
                throw new ApplicationException("AI returned null roadmap");
            return response;
        }
    }
}
