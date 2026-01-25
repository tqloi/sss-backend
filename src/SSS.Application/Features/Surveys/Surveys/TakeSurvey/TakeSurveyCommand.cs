using MediatR;
using System.Text.Json.Serialization;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Surveys.Surveys.TakeSurvey
{
    public sealed record TakeSurveyCommand : IRequest<TakeSurveyResponse>
    {
        [JsonIgnore]
        public long SurveyId { get; init; }

        [JsonIgnore]
        public string UserId { get; init; } = default!;

        public DateTime StartedAt { get; init; }
        public DateTime? SubmittedAt { get; init; }
        public SurveyTriggerReason TriggerReason { get; init; }
        public List<SurveyAnswerInput> Answers { get; init; } = new();
    }

    public sealed record SurveyAnswerInput
    {
        public long QuestionId { get; init; }
        public long? OptionId { get; init; }
        public decimal? NumberValue { get; init; }
        public string? TextValue { get; init; }
        public DateTime AnsweredAt { get; init; }
    }
}