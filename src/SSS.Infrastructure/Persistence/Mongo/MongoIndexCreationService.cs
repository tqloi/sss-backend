using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo
{
    public class MongoIndexCreationService : IHostedService
    {
        private readonly MongoContext _context;
        private readonly ILogger<MongoIndexCreationService> _logger;

        public MongoIndexCreationService(MongoContext context, ILogger<MongoIndexCreationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var collection = _context.GetCollection<PostHogEventDocument>("posthog_events");

                var indexes = new[]
                {
                    new CreateIndexModel<PostHogEventDocument>(
                        Builders<PostHogEventDocument>.IndexKeys.Ascending("raw_payload.event")),
                    new CreateIndexModel<PostHogEventDocument>(
                        Builders<PostHogEventDocument>.IndexKeys.Ascending("raw_payload.distinct_id")),
                    new CreateIndexModel<PostHogEventDocument>(
                        Builders<PostHogEventDocument>.IndexKeys.Descending("raw_payload.timestamp")),
                    new CreateIndexModel<PostHogEventDocument>(
                        Builders<PostHogEventDocument>.IndexKeys.Descending(e => e.ReceivedAt))
                };

                await collection.Indexes.CreateManyAsync(indexes, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Mongo index creation skipped during startup.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
