using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.Users.Profile.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResult>
{
    private readonly UserManager<User> _userManager;

    public ChangePasswordHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ChangePasswordResult> Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new KeyNotFoundException("User not found");

        // Validate current password
        var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
        if (!isCurrentPasswordValid)
            throw new UnauthorizedAccessException("Current password is incorrect");

        // Change password (Identity will hash the new password automatically)
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return new ChangePasswordResult
        {
            Success = true,
            Message = "Password changed successfully"
        };
    }
}
