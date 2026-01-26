using MediatR;

namespace SSS.Application.Features.AI.CreateRoadMap
{
    public sealed record CreateRoadMapCommand(string Message)
        : IRequest<CreateRoadMapResult>;
}