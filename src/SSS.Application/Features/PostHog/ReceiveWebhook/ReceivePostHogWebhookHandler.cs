using MediatR;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Domain.Entities.Tracking;
using System;
using System.Text.Json;

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

            JsonElement? GetProperty(JsonElement? doc, string key)
            {
                if (!doc.HasValue || doc.Value.ValueKind != JsonValueKind.Object) return null;
                if (doc.Value.TryGetProperty(key, out var val)) return val;
                
                // Try camelCase if snake_case failed
                var camelKey = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(key.Replace("_", " "));
                if (doc.Value.TryGetProperty(camelKey, out val)) return val;

                return null;
            }

            string? GetString(JsonElement? doc, string key)
            {
                var val = GetProperty(doc, key);
                if (val == null || val.Value.ValueKind == JsonValueKind.Null) return null;
                return val.Value.ValueKind == JsonValueKind.String ? val.Value.GetString() : val.Value.GetRawText().Trim('\"');
            }

            double? GetDouble(JsonElement? doc, string key)
            {
                var val = GetProperty(doc, key);
                if (val == null || val.Value.ValueKind == JsonValueKind.Null) return null;
                if (val.Value.ValueKind == JsonValueKind.Number) return val.Value.GetDouble();
                if (val.Value.ValueKind == JsonValueKind.String && double.TryParse(val.Value.GetString(), out var d)) return d;
                return null;
            }

            bool? GetBool(JsonElement? doc, string key)
            {
                var val = GetProperty(doc, key);
                if (val == null || val.Value.ValueKind == JsonValueKind.Null) return null;
                if (val.Value.ValueKind == JsonValueKind.True) return true;
                if (val.Value.ValueKind == JsonValueKind.False) return false;
                if (val.Value.ValueKind == JsonValueKind.String && bool.TryParse(val.Value.GetString(), out var b)) return b;
                return null;
            }

            DateTime? GetDateTime(JsonElement? doc, string key)
            {
                var val = GetProperty(doc, key);
                if (val == null || val.Value.ValueKind != JsonValueKind.String) return null;
                return val.Value.TryGetDateTime(out var dt) ? dt : null;
            }

            var propertiesDoc = raw.ValueKind == JsonValueKind.Object && raw.TryGetProperty("properties", out var p) && p.ValueKind == JsonValueKind.Object ? p : (JsonElement?)null;

            // Fallback chain for properties: check properties object, then top-level raw
            string? FetchString(string key) => GetString(propertiesDoc, key) ?? GetString(raw, key);
            double? FetchDouble(string key) => GetDouble(propertiesDoc, key) ?? GetDouble(raw, key);
            bool? FetchBool(string key) => GetBool(propertiesDoc, key) ?? GetBool(raw, key);
            DateTime? FetchDateTime(string key) => GetDateTime(propertiesDoc, key) ?? GetDateTime(raw, key);

            var evt = new PostHogEvent
            {
                EventId = FetchString("event_id"),
                EventName = FetchString("event_name"),
                OccurredAt = FetchDateTime("occurred_at"),
                DistinctId = FetchString("distinct_id"),

                // Top-level fields
                UserId = FetchString("user_id") ?? FetchString("distinct_id"),
                SessionId = FetchString("learning_session_id"),
                PostHogSessionId = FetchString("posthog_session_id"),
                StudyPlanId = FetchString("study_plan_id"),
                NodeId = FetchString("node_id"),
                TaskId = FetchString("task_id"),
                ContentId = FetchString("content_id"),

                Properties = new PostHogEventProperties
                {
                    Subject = FetchString("subject"),
                    Topic = FetchString("topic"),
                    DurationMinutes = FetchDouble("duration_minutes"),
                    IsCorrect = FetchBool("is_correct"),
                    TimeSpentSeconds = FetchDouble("time_spent_seconds"),
                    PlannedDurationSeconds = FetchDouble("planned_duration_seconds"),
                    CorrectRate = FetchDouble("correct_rate"),
                    CurrentUrl = FetchString("current_url"),
                    Browser = FetchString("browser"),
                    Os = FetchString("os")
                },
                ReceivedAt = DateTime.UtcNow
            };

            await _postHogEventRepository.AddAsync(evt);
        }
    }
}
