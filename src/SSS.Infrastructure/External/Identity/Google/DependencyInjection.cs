using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SSS.Infrastructure.External.Identity.Google
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGoogleAuthService(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            var section = config.GetSection("Authentication:Google");
            var s = section.Get<GoogleAuthOptions>()
            ?? throw new InvalidOperationException("Authentication:Google section is missing or invalid.");

            services
            .AddAuthentication()
            .AddGoogle((options) =>
            {
                options.ClientId = s.ClientId;
                options.ClientSecret = s.ClientSecret;
                options.CallbackPath = "/signin-google";
                // Explicitly request the profile scope
                options.Scope.Add("profile");

                // Map the picture to a claim
                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                options.Events.OnRemoteFailure = context =>
                {
                    context.Response.Redirect("/api/auth/google-callback?error=access_denied");
                    context.HandleResponse(); // chặn exception mặc định
                    return Task.CompletedTask;
                };
            });
            services.Configure<GoogleAuthOptions>(section);
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            return services;
        }
    }
}
