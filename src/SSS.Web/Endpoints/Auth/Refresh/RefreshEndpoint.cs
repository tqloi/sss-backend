using FastEndpoints;
using SSS.Application.Features.Auth.Refresh;
using SSS.WebApi.Endpoints.Auth.Common;
using MediatR;

namespace SSS.WebApi.Endpoints.Auth.Refresh
{
    public sealed class RefreshEndpoint(
        ISender sender,
        IWebHostEnvironment env,
        AutoMapper.IMapper mapper
        ) : EndpointWithoutRequest<RefreshResponse>
    {
        public override void Configure()
        {
            Post("/api/auth/refresh");
            AllowAnonymous();
            Description(d => d.WithTags("Auth"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var refreshPlain = HttpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshPlain))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var returnUrl = HttpContext.Request.Query["returnUrl"].FirstOrDefault() ?? "/";

            var command = new RefreshCommand
            {
                RefreshTokenPlain = refreshPlain,
                RequestIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                ReturnUrl = returnUrl
            };

            var result = await sender.Send(command, ct);

            // Set lại cookie với token mới (HttpOnly)
            AuthHelper.AppendRefreshCookie(HttpContext, result.RefreshTokenPlain, result.RefreshTokenExpiresUtc, env.IsDevelopment());

            var res = mapper.Map<RefreshResponse>(result);

            await SendOkAsync(res, ct);
        }
    }
}