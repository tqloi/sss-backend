using MediatR;

namespace SSS.Application.Features.AI.CreateAiRoadMap
{
    public sealed record CreateAiRoadMapCommand(string Message, string ManagerId, long SubjectId)
        : IRequest<CreateAiRoadMapResult>;
}