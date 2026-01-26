using MediatR;
using SSS.Application.Abstractions.External.AI.PipeLine;

namespace SSS.Application.Features.AI.CreateRoadMap
{
    public sealed class CreateRoadMapHandler(IPipeLine pipeLine)
        : IRequestHandler<CreateRoadMapCommand, CreateRoadMapResult>
    {
        public async Task<CreateRoadMapResult> Handle(CreateRoadMapCommand request, CancellationToken cancellationToken)
        {
            var rawJson = await pipeLine.AskAsync(
                request.Message,
                topK: 5,
                ct: cancellationToken
            );
            // 2. Deserialize
            var response = new CreateRoadMapResult(rawJson);
            if (response is null)
                throw new ApplicationException("AI returned null roadmap");
            return response;
        }
    }
}
