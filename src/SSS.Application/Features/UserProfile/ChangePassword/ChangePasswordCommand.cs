using MediatR;

namespace SSS.Application.Features.UserProfile.ChangePassword;

public sealed class ChangePasswordCommand : IRequest<ChangePasswordResult>
{
    public string UserId { get; set; } = default!;
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmNewPassword { get; set; } = default!;
}
