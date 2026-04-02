using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.UserProfile.ChangePassword;

public sealed class ChangePasswordHandler(
    UserManager<User> userManager
) : IRequestHandler<ChangePasswordCommand, ChangePasswordResult>
{
    public async Task<ChangePasswordResult> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        // Validate new password and confirmation match
        if (request.NewPassword != request.ConfirmNewPassword)
            throw new InvalidOperationException("New password and confirmation password do not match");

        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new InvalidOperationException("User not found");

        // Verify current password
        var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, request.CurrentPassword);
        if (!isCurrentPasswordValid)
            throw new InvalidOperationException("Current password is incorrect");

        // Change password (this hashes the new password automatically)
        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(result.Errors.First().Description);

        return new ChangePasswordResult
        {
            Success = true,
            Message = "Password changed successfully"
        };
    }
}
