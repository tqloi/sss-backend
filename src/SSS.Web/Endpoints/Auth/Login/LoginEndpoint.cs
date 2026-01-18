using SSS.Application.Features.Auth.Login;
using SSS.WebApi.Endpoints.Auth.Common;
using FastEndpoints;
using MediatR;

namespace SSS.WebApi.Endpoints.Auth.Login
{
    public sealed class LoginEndpoint(
         ISender sender,
         IWebHostEnvironment env,
         AutoMapper.IMapper mapper
        ) : Endpoint<LoginRequest, LoginResponse>
    {
        public override void Configure()
        {
            Post("/api/auth/login");
            AllowAnonymous();
            Description(d => d.WithTags("Auth"));
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var command = mapper.Map<LoginCommand>(req);
            command.RequestIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            var result = await sender.Send(command, ct);

            // success: set cookie rồi trả 200
            AuthHelper.AppendRefreshCookie(HttpContext, result.RefreshTokenPlain, result.RefreshTokenExpiresUtc, env.IsDevelopment());

            var res = mapper.Map<LoginResponse>(result)!;

            await SendOkAsync(res, ct);
        }
    }
}
