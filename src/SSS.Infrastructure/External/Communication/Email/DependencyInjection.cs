using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSS.Application.Abstractions.External.Communication.Email;

namespace SSS.Infrastructure.External.Communication.Email
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMailService(
        this IServiceCollection services,
        IConfiguration config
        )
        {
            var mailConfigs = config.GetSection("MailSettings");
            services.Configure<EmailOptions>(mailConfigs);
            services.AddTransient<ISmtpEmailSender, SmtpEmailSender>();
            services.AddScoped<IMailTemplateBuilder, MailTemplateBuilder>();

            return services;
        }
    }
}
