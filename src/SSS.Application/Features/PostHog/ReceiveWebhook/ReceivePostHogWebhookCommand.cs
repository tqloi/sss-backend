using MediatR;
using MongoDB.Bson;

namespace SSS.Application.Features.PostHog.ReceiveWebhook
{
    public class ReceivePostHogWebhookCommand : IRequest
    {
        public BsonDocument RawPayload { get; set; } = new();
    }
}
