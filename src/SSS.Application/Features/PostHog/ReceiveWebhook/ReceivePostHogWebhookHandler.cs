using MediatR;
using MongoDB.Bson;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.Tracking;
using System;

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

            string? GetString(BsonDocument? doc, string key) =>
                doc != null && doc.TryGetValue(key, out var val) && val.BsonType == BsonType.String
                    ? val.AsString : null;

            double? GetDouble(BsonDocument? doc, string key) =>
                doc != null && doc.TryGetValue(key, out var val) && val.IsNumeric
                    ? val.AsDouble : null;

            bool? GetBool(BsonDocument? doc, string key) =>
                doc != null && doc.TryGetValue(key, out var val) && val.IsBoolean
                    ? val.AsBoolean : null;

            DateTime? GetDateTime(BsonDocument? doc, string key) =>
                doc != null && doc.TryGetValue(key, out var val) && val.BsonType == BsonType.String
                    && DateTime.TryParse(val.AsString, out var dt) ? dt : null;

            var propertiesDoc = raw.TryGetValue("properties", out var p) && p.IsBsonDocument ? p.AsBsonDocument : null;

            var evt = new PostHogEvent
            {
                EventId = GetString(raw, "uuid") ?? GetString(raw, "id"),
                EventName = GetString(raw, "event"),
                OccurredAt = GetDateTime(raw, "timestamp"),
                DistinctId = GetString(raw, "distinct_id"),

                // Fields from properties
                UserId = GetString(propertiesDoc, "userId") ?? GetString(propertiesDoc, "user_id") ?? GetString(raw, "distinct_id"),
                SessionId = GetString(propertiesDoc, "sessionId") ?? GetString(propertiesDoc, "session_id"),
                PostHogSessionId = GetString(propertiesDoc, "$session_id"),
                StudyPlanId = GetString(propertiesDoc, "studyPlanId") ?? GetString(propertiesDoc, "study_plan_id"),
                NodeId = GetString(propertiesDoc, "nodeId") ?? GetString(propertiesDoc, "node_id"),
                TaskId = GetString(propertiesDoc, "taskId") ?? GetString(propertiesDoc, "task_id"),
                ContentId = GetString(propertiesDoc, "contentId") ?? GetString(propertiesDoc, "content_id"),

                Properties = propertiesDoc != null ? new PostHogEventProperties
                {
                    Subject = GetString(propertiesDoc, "subject"),
                    Topic = GetString(propertiesDoc, "topic"),
                    DurationMinutes = GetDouble(propertiesDoc, "durationMinutes") ?? GetDouble(propertiesDoc, "duration_minutes"),
                    IsCorrect = GetBool(propertiesDoc, "isCorrect") ?? GetBool(propertiesDoc, "is_correct"),
                    TimeSpentSeconds = GetDouble(propertiesDoc, "timeSpentSeconds") ?? GetDouble(propertiesDoc, "time_spent_seconds"),
                    CorrectRate = GetDouble(propertiesDoc, "correctRate") ?? GetDouble(propertiesDoc, "correct_rate"),
                    CurrentUrl = GetString(propertiesDoc, "$current_url"),
                    Browser = GetString(propertiesDoc, "$browser"),
                    Os = GetString(propertiesDoc, "$os")
                } : null,
                ReceivedAt = DateTime.UtcNow
            };

            await _postHogEventRepository.AddAsync(evt);
        }
    }
}
