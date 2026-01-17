using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SSS.Infrastructure.External.Communication.Email;
using SSS.Infrastructure.External.Identity.Google;
using SSS.Infrastructure.Persistence.Sql;
using SSS.Infrastructure.Sercurity.Jwt;
using System.Reflection;

namespace SSS.Infrastructure
{
    public static class DependencyInjection
    {
        //[Obsolete]
        public static IServiceCollection AddCoreInfrastructure(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddDatabase(config);
            services.AddJwtAuthentication(config);
            services.AddMailService(config);
            services.AddGoogleAuthService(config);
            //services.AddGoogleStorage(config);
            //services.AddPayOSService(config);
            //services.AddSwaggerWithAuth();
            //services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            //services.AddFastEndpoints();
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCourseHandler).Assembly));
            //services.AddScopedServicesByConvention
            //(
            //     appAssembly: typeof(ICertificateIssuanceService).Assembly,
            //     infraAssembly: typeof(CertificateIssuanceService).Assembly
            //);

            //// Certificate background workers
            //services.AddSingleton<StudentCourseCompletionQueue>();
            //services.AddSingleton<IStudentCourseCompletionQueue>(sp => sp.GetRequiredService<StudentCourseCompletionQueue>());
            //services.AddHostedService<CertificateIssueWorker>();
            //services.AddHttpClient<IFirebaseDbService, FirebaseDbService>();
            //services.AddODataSupport();

            // FastEndpoints
            services.AddFastEndpoints();
            services.SwaggerDocument(o =>
            {
                o.AutoTagPathSegmentIndex = 2;
                o.DocumentSettings = s =>
                {
                    s.Title = "sss-backend";
                    s.Version = "v1"; // -> /swagger/v1/swagger.json
                };

                // (tuỳ version, có thể có o.EnableJWTBearerAuth = true; nhưng v5.24 dùng AddAuth như trên là chắc)
            });

            return services;
        }

        private static IServiceCollection AddScopedServicesByConvention(
            this IServiceCollection services,
            Assembly appAssembly,
            Assembly infraAssembly,
            string[]? allowedSuffixes = null)
        {
            allowedSuffixes ??= new[] { "Service", "Repository", "Provider", "Generator", "Client", "Gateway" };

            var appTypes = appAssembly.GetTypes();
            var infraTypes = infraAssembly.GetTypes();

            // 1) Lấy tất cả interface hợp lệ trong Application
            var interfaces = appTypes
                .Where(t =>
                    t.IsInterface &&
                    t.Name.StartsWith("I", StringComparison.Ordinal) &&
                    allowedSuffixes.Any(suf => t.Name.EndsWith(suf, StringComparison.Ordinal)))
                .ToArray();

            foreach (var iface in interfaces)
            {
                // 2) Tìm implementation trong Infrastructure
                var impls = infraTypes
                    .Where(c =>
                        c.IsClass &&
                        !c.IsAbstract &&
                        iface.IsAssignableFrom(c))
                    .ToArray();

                if (impls.Length == 0)
                    continue;

                // Ưu tiên map theo tên: IFooService → FooService
                var expectedName = iface.Name.StartsWith("I", StringComparison.Ordinal)
                    ? iface.Name.Substring(1)
                    : iface.Name;

                var preferred = impls.FirstOrDefault(c => string.Equals(c.Name, expectedName, StringComparison.Ordinal))
                               ?? impls.First();

                // 3) Đăng ký
                if (iface.IsGenericTypeDefinition && preferred.IsGenericTypeDefinition)
                {
                    // Open generic: IRepo<> -> Repo<>
                    services.TryAdd(new ServiceDescriptor(iface, preferred, ServiceLifetime.Scoped));
                }
                else
                {
                    services.TryAddScoped(iface, preferred);
                }
            }

            return services;
        }
    }
}
