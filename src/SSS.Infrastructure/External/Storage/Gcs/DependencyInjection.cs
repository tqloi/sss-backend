using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace SSS.Infrastructure.External.Storage.Gcs
{
    public static class DependencyInjection
    {
        [Obsolete]
        public static IServiceCollection AddGcsStorage(
            this IServiceCollection services,
            IConfiguration config)
        {
            // Bind setting (Bucket, DefaultAvatarPath, KeyPath)
            services.Configure<GcsStorageOptions>(config.GetSection("GcpStorage"));

            // GoogleCredential (auto dùng ADC nếu không có KeyPath)
            services.AddSingleton(sp =>
            {
                var opt = sp.GetRequiredService<IOptions<GcsStorageOptions>>().Value;
                var cred = string.IsNullOrWhiteSpace(opt.KeyPath)
                    ? GoogleCredential.GetApplicationDefault()
                    : GoogleCredential.FromFile(opt.KeyPath);
                return StorageClient.Create(cred);
            });

            // UrlSigner
            services.AddSingleton(sp =>
            {
                var opt = sp.GetRequiredService<IOptions<GcsStorageOptions>>().Value;
                var keyPath = opt.KeyPath ?? throw new("Missing GcpStorage:KeyPath for UrlSigner");
                var sa = ServiceAccountCredential.FromServiceAccountData(File.OpenRead(keyPath));
                return UrlSigner.FromServiceAccountCredential(sa);
            });

            // Service triển khai
            services.AddScoped<IGcsStorageService, GcsStorageService>();

            return services;
        }
    }
}
