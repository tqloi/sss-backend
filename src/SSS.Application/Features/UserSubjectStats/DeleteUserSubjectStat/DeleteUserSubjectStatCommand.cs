using MediatR;

namespace SSS.Application.Features.UserSubjectStats.DeleteUserSubjectStat;

public sealed class DeleteUserSubjectStatCommand : IRequest<bool>
{
    public long Id { get; set; }
    public string UserId { get; set; } = default!;
}
