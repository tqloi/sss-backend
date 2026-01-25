using MediatR;

namespace SSS.Application.Features.AI.CreateRoadMap
{
    public sealed record CreateRoadMapRequest(string Message)
        : IRequest<CreateRoadMapResponse>;
}