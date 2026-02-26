using MediatR;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.GetPendingTriggerSurvey
{
    public sealed record GetPendingTriggerSurveyQuery(
        string TriggerType,
        string UserId
    ) : IRequest<GetPendingTriggerSurveyResult>;
}
