using MediatR;
using SSS.Application.Features.UserSubjectStats.Common;

namespace SSS.Application.Features.UserSubjectStats.GetUserSubjectStats;

public sealed class GetUserSubjectStatsQuery : IRequest<List<UserSubjectStatDto>>
{
    public string UserId { get; set; } = default!;
}
