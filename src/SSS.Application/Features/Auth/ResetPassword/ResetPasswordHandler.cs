using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.Auth.ResetPassword
{
    public sealed class ResetPasswordHandler(
        UserManager<User> userManager)
        : IRequestHandler<ResetPasswordCommand>
    {
        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new InvalidOperationException("User does not exist");

            var decodedBytes = WebEncoders.Base64UrlDecode(request.Token);
            var rawToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await userManager.ResetPasswordAsync(user, rawToken, request.NewPassword);
            if (!result.Succeeded)
                throw new InvalidOperationException(result.Errors.First().Description);

            return;
        }
    }
}
