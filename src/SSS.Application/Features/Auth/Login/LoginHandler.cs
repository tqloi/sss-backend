using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Application.Abstractions.Sercurity.Jwt;
using SSS.Application.Features.Auth.Common;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.Auth.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtService;

        public LoginHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtTokenService jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwt;
        }

        public async Task<AuthResult> Handle(LoginCommand req, CancellationToken ct)
        {
            var user = await _userManager.FindByNameAsync(req.EmailOrUserName)
                       ?? await _userManager.FindByEmailAsync(req.EmailOrUserName);
            if (user is null)
                throw new UnauthorizedAccessException("User does not exist");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new UnauthorizedAccessException("Email not confirmed");

            var pwResult = await _signInManager.CheckPasswordSignInAsync(user, req.Password, lockoutOnFailure: true);

            if (!pwResult.Succeeded)
                throw new UnauthorizedAccessException("Wrong password");

            return await _jwtService.IssueAsync(user, req.RequestIp, ct);
        }
    }
}
