using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using SSS.Domain.Entities.Identity;

public sealed class GoogleLoginEndpoint(SignInManager<User> signInManager)
  : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/auth/google-login");
        AllowAnonymous();
        DontAutoSendResponse();
        Description(d => d.WithTags("Auth"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        //var returnUrl = Query<string>("returnUrl") ?? "/";
        var returnUrl = HttpContext.Request.Query["returnUrl"].FirstOrDefault() ?? "/";

        var req = HttpContext.Request;
        var redirectUrl =
            $"{req.Scheme}://{req.Host}{req.PathBase}{GoogleCallbackEndpoint.Route}" +
            $"?returnUrl={Uri.EscapeDataString(returnUrl)}";

        var props = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        await HttpContext.ChallengeAsync("Google", props);
    }
}
