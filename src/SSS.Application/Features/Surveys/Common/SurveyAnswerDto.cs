namespace SSS.Application.Features.Surveys.Common
{
    public sealed record SurveyAnswerDto(
        long Id,
        long ResponseId,
        long QuestionId,
        long? OptionId,
        decimal? NumberValue,
        string? TextValue,
        DateTime AnsweredAt
    );
}