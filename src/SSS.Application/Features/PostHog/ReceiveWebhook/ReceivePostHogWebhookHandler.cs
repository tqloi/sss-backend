using MediatR;
using MongoDB.Bson;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Features.PostHog.ReceiveWebhook
{
    public class ReceivePostHogWebhookHandler : IRequestHandler<ReceivePostHogWebhookCommand>
    {
        private readonly IPostHogEventRepository _postHogEventRepository;

        public ReceivePostHogWebhookHandler(IPostHogEventRepository postHogEventRepository)
        {
            _postHogEventRepository = postHogEventRepository;
        }

        public async Task Handle(ReceivePostHogWebhookCommand request, CancellationToken cancellationToken)
        {
            var raw = request.RawPayload;

            string? GetString(string key) =>
                raw.TryGetValue(key, out var val) && val.BsonType == BsonType.String
                    ? val.AsString : null;

            DateTime? GetDateTime(string key) =>
                raw.TryGetValue(key, out var val) && val.BsonType == BsonType.String
                    && DateTime.TryParse(val.AsString, out var dt) ? dt : null;

            var evt = new PostHogEvent
            {                
                Properties = raw.TryGetValue("properties", out var p) && p.BsonType == BsonType.Document
                    ? p.AsBsonDocument : null,
                RawPayload = raw,
                ReceivedAt = DateTime.UtcNow
            };

            await _postHogEventRepository.AddAsync(evt);
        }
    }
}
