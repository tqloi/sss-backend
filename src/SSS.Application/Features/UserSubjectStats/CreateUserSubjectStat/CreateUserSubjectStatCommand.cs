using MediatR;
using SSS.Application.Features.UserSubjectStats.Common;

namespace SSS.Application.Features.UserSubjectStats.CreateUserSubjectStat;

public sealed class CreateUserSubjectStatCommand : IRequest<UserSubjectStatDto>
{
    public string UserId { get; set; } = default!;
    public long SubjectId { get; set; }
    public decimal? ProficiencyLevel { get; set; }
    public decimal? TotalHoursSpent { get; set; }
    public string? WeakNodeIds { get; set; }
}
