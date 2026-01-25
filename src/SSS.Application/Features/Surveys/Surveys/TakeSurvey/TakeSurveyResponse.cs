using SSS.Application.Common.Dtos;

namespace SSS.Application.Features.Surveys.Surveys.TakeSurvey
{
    public sealed record TakeSurveyResponse(
        bool Success,
        string Message,
        TakeSurveyData? Data = null) : GenericResponseRecord<TakeSurveyData>(Success, Message, Data);

    public sealed record TakeSurveyData
    {
        public long ResponseId { get; init; }
        public string Status { get; init; } = default!; // "InProgress" or "Completed"
        public int AnsweredCount { get; init; }
        public int TotalQuestions { get; init; }
        public List<string>? ValidationErrors { get; init; }
    }
}