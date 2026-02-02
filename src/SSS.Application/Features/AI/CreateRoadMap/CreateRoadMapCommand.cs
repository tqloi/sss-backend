using MediatR;

namespace SSS.Application.Features.AI.CreateRoadMap
{
    public sealed record CreateRoadMapCommand(string Message, string subjectid)
        : IRequest<CreateRoadMapResult>;
}