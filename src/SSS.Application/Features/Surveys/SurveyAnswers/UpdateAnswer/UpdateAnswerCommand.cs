using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Surveys.SurveyAnswers.UpdateAnswer
{
    public sealed record UpdateAnswerCommand : IRequest<UpdateAnswerResponse>
    {
        [JsonIgnore]
        public long AnswerId { get; init; }
        
        public long? OptionId { get; init; }
        public decimal? NumberValue { get; init; }
        public string? TextValue { get; init; }
    }
}