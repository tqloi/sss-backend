using MediatR;
using SSS.Application.Features.UserSubjectStats.Common;

namespace SSS.Application.Features.UserSubjectStats.UpdateUserSubjectStat;

public sealed class UpdateUserSubjectStatCommand : IRequest<UserSubjectStatDto>
{
    public long Id { get; set; }
    public string UserId { get; set; } = default!;
    public decimal? ProficiencyLevel { get; set; }
    public decimal? TotalHoursSpent { get; set; }
    public string? WeakNodeIds { get; set; }
}
