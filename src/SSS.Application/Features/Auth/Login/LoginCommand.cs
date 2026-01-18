using SSS.Application.Features.Auth.Common;
using MediatR;

namespace SSS.Application.Features.Auth.Login
{
    public sealed class LoginCommand : IRequest<AuthResult>
    {
        public string EmailOrUserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? ReturnUrl { get; set; }

        // đưa IP từ Endpoint vào đây (hoặc dùng Behavior)
        public string? RequestIp { get; set; }
    }
}
