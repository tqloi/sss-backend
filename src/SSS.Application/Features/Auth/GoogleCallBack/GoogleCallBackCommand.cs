using SSS.Application.Features.Auth.Common;
using MediatR;

namespace SSS.Application.Features.Auth.GoogleCallBack
{
    public sealed record GoogleCallbackCommand(
        string? RequestIp
    ) : IRequest<AuthResult>;
}
