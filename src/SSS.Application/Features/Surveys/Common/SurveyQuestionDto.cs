using SSS.Domain.Enums;

namespace SSS.Application.Features.Surveys.Common
{
    public sealed record SurveyQuestionDto
    (
        long Id,
        long SurveyId,
        string QuestionKey,
        string Prompt,
        SurveyQuestionType Type,
        int OrderNo,
        bool IsRequired,
        int? ScaleMin,
        int? ScaleMax
    );

    
}
