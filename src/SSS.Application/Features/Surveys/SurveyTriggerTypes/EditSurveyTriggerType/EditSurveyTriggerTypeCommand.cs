using MediatR;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.EditSurveyTriggerType
{
    public sealed record EditSurveyTriggerTypeCommand(
        string Code,
        string DisplayName,
        string? Description,
        bool IsActive
    ) : IRequest<EditSurveyTriggerTypeResponse>;
}
