using FastEndpoints;
using MediatR;
using SSS.Application.Features.Auth.Logout;

namespace SSS.WebApi.Endpoints.Auth.Logout;

public sealed class LogoutEndpoint(ISender sender)
    : EndpointWithoutRequest<LogoutResponse>
{
    public override void Configure()
    {
        Post("/api/auth/logout");
        Description(d => d.WithTags("Auth"));
        AllowAnonymous(); // hoặc dùng Authorize()
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        // 1) Lấy refresh token từ cookie + IP
        var refreshPlain = HttpContext.Request.Cookies["refreshToken"];
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

        // 2) Gọi Handler (Application)
        var result = await sender.Send(new LogoutCommand
        {
            RefreshTokenPlain = refreshPlain,
            RequestIp = ip
        }, ct);

        var res = new LogoutResponse
        {
            Message = result.Message
        };

        // 3) Xoá cookie phía client (HTTP concern ở WebApi)
        HttpContext.Response.Cookies.Delete("refreshToken");

        await SendOkAsync(res, ct);
    }
}
