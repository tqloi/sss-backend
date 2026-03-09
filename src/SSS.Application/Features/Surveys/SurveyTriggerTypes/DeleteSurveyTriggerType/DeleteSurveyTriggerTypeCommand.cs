using MediatR;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.DeleteSurveyTriggerType
{
    public sealed record DeleteSurveyTriggerTypeCommand(string Code) : IRequest<DeleteSurveyTriggerTypeResponse>;
}
