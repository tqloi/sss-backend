using MediatR;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.CreateSurveyTriggerType
{
    public sealed record CreateSurveyTriggerTypeCommand(
        string Code,
        string DisplayName,
        string? Description,
        bool IsActive = true
    ) : IRequest<CreateSurveyTriggerTypeResponse>;
}
