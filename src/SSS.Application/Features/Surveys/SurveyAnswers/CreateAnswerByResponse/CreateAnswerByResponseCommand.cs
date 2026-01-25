using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Surveys.SurveyAnswers.CreateAnswerByResponse
{
    public sealed record CreateAnswerByResponseCommand : IRequest<CreateAnswerByResponseResponse>
    {
        [JsonIgnore]
        public long ResponseId { get; init; }
        
        public long QuestionId { get; init; }
        public long? OptionId { get; init; }
        public decimal? NumberValue { get; init; }
        public string? TextValue { get; init; }
        public DateTime AnsweredAt { get; init; } = DateTime.UtcNow;
    }
}