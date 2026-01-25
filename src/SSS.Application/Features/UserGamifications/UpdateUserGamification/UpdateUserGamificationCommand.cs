using MediatR;
using SSS.Application.Features.UserGamifications.Common;

namespace SSS.Application.Features.UserGamifications.UpdateUserGamification;

public sealed class UpdateUserGamificationCommand : IRequest<UserGamificationDto>
{
    public string UserId { get; set; } = default!;
    public int? CurrentStreak { get; set; }
    public int? LongestStreak { get; set; }
    public DateTime? LastActiveDate { get; set; }
    public int? TotalExp { get; set; }
}
