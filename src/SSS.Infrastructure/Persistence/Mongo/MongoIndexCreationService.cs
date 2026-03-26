using System.Text.Json;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using SSS.Infrastructure.Persistence.Mongo.Documents;

namespace SSS.Infrastructure.Persistence.Mongo
{
    public class MongoIndexCreationService : IHostedService
    {
        private readonly MongoContext _context;

        public MongoIndexCreationService(MongoContext context)
        {
            _context = context;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
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

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
