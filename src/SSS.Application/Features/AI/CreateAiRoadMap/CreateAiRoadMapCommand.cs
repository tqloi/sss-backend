using MediatR;

namespace SSS.Application.Features.AI.CreateAiRoadMap
{
    public sealed record CreateAiRoadMapCommand(string Message, string subjectid)
        : IRequest<CreateAiRoadMapResult>;
}