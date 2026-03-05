namespace SSS.Application.Features.Surveys.Common
{
    public sealed record SurveyTriggerTypeDto
    (
        string Code,
        string DisplayName,
        string? Description,
        bool IsActive
    );
}
