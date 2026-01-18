using FastEndpoints;
using SSS.Application.Features.Auth.GoogleCallBack;
using SSS.WebApi.Endpoints.Auth.Common;
using SSS.WebApi.Endpoints.Auth.GoogleLogin;
using MediatR;

public sealed class GoogleCallbackEndpoint(
    ISender sender,
    IWebHostEnvironment env
) : Endpoint<GoogleCallbackRequest>
{
    public const string Route = "/api/auth/google-callback"; 
    public override void Configure()
    {
        Get(Route);
        AllowAnonymous();
        Description(d => d.WithTags("Auth"));
    }

    public override async Task HandleAsync(GoogleCallbackRequest req, CancellationToken ct)
    {
        try
        {
            var result = await sender.Send(new GoogleCallbackCommand(
                 RequestIp: HttpContext.Connection.RemoteIpAddress?.ToString()
             ), ct);

            // set refresh cookie (HTTP concern)
            AuthHelper.AppendRefreshCookie(HttpContext, result.RefreshTokenPlain, result.RefreshTokenExpiresUtc, env.IsDevelopment());

            // postMessage & close popup
            var payloadJson = System.Text.Json.JsonSerializer.Serialize(new
            {
                token = result.AccessToken,
                user = result.User,
                returnUrl = req.ReturnUrl
            });

            var script = $@"(function(){{
                var params = new URLSearchParams(window.location.search);
                var openerFromQuery = params.get('opener');
                var openerOrigin = openerFromQuery || (document.referrer ? new URL(document.referrer).origin : '*');
                var payload = {payloadJson};
                if (window.opener && !window.opener.closed) {{
                    window.opener.postMessage(payload, openerOrigin);
                }}
                setTimeout(function(){{ window.close(); }}, 0);
            }})();";

            await SendHtmlAndClose(script);
        }
        catch (InvalidOperationException ex)
        {
            var errorJson = System.Text.Json.JsonSerializer.Serialize(new
            {
                error = new { message = ex.Message }
            });

            await SendHtmlAndClose($@"window.opener?.postMessage({errorJson}, '*');");
            return;
        }
        catch (Exception ex)
        {
            var errorJson = System.Text.Json.JsonSerializer.Serialize(new
            {
                error = new { message = ex.Message }
            });

            await SendHtmlAndClose($@"window.opener?.postMessage({errorJson}, '*');");
            return;
        }
    }

    private async Task SendHtmlAndClose(string inlineJs)
    {
        HttpContext.Response.ContentType = "text/html; charset=utf-8";
        await HttpContext.Response.WriteAsync($@"<!doctype html><html><body><script>{inlineJs}</script></body></html>");
    }
}