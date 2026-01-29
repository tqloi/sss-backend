using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SSS.Application.Abstractions.Sercurity.Jwt;
using System.Text;

namespace SSS.Infrastructure.Sercurity.Jwt
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("Jwt");
            var s = section.Get<JwtOptions>()
                     ?? throw new InvalidOperationException("Jwt section is missing or invalid.");

            // --- VALIDATION (fail-fast) ---
            if (string.IsNullOrWhiteSpace(s.Key) || s.Key.Length < 32)
                throw new InvalidOperationException("Jwt:Secret must be at least 32 characters.");
            if (string.IsNullOrWhiteSpace(s.Issuer))
                throw new InvalidOperationException("Jwt:Issuer is required.");
            if (string.IsNullOrWhiteSpace(s.Audience))
                throw new InvalidOperationException("Jwt:Audience is required.");

            // Nếu bạn muốn inject IOptions<JwtOptions> ở nơi khác:
            services.Configure<JwtOptions>(section);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = s.Issuer,
                        ValidAudience = s.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s.Key)),
                    };
                });

            services.AddAuthorization();
            //services.AddScoped<IJwtTokenService, JwtTokenService>();
            return services;
        }
    }
}
