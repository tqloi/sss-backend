using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Infrastructure.Persistence.Mongo.Repositories;

namespace SSS.Infrastructure.Persistence.Mongo
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMongo(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("MongoDB:ConnectionString");

            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentNullException("MongoDB:DatabaseName");

            services.AddSingleton<IMongoClient>(_ =>
                    new MongoClient(connectionString));

            services.AddSingleton<MongoContext>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return new MongoContext(client, databaseName);
            });

            // Mongo Repositories
            services.AddScoped<IStudyEventRepository, StudyEventRepository>();
            services.AddScoped<IPostHogEventRepository, PostHogEventRepository>();

            services.AddHostedService<MongoIndexCreationService>();

            return services;
        }
    }
}
