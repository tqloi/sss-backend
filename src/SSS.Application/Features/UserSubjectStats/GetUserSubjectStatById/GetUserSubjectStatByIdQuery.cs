using MediatR;
using SSS.Application.Features.UserSubjectStats.Common;

namespace SSS.Application.Features.UserSubjectStats.GetUserSubjectStatById;

public sealed class GetUserSubjectStatByIdQuery : IRequest<UserSubjectStatDto?>
{
    public long Id { get; set; }
    public string UserId { get; set; } = default!;
}
