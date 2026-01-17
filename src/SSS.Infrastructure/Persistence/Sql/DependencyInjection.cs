using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Entities.Identity;

namespace SSS.Infrastructure.Persistence.Sql
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            var connectionString =
                config.GetConnectionString("sqlConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'sqlConnection' not found."
                );

            services.AddDbContext<AppDbContext>(options => options.UseMySQL(connectionString));
            // Cấu hình Identity
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredLength = 6;
                    options.User.RequireUniqueEmail = true;

                    // Lockout configuration
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Locked for 5 minutes
                    options.Lockout.MaxFailedAccessAttempts = 5; // Lock after 5 failed attempts
                    options.Lockout.AllowedForNewUsers = true;

                    // User configuration
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                    // SignIn configuration
                    options.SignIn.RequireConfirmedEmail = true; // Email must exist
                    options.SignIn.RequireConfirmedPhoneNumber = false; //
                })

                //.AddRoles<IdentityRole>() 
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
            services.Configure<DataProtectionTokenProviderOptions>(o =>
            {
                o.TokenLifespan = TimeSpan.FromHours(2);
            });

            return services;
        }
    }
}