using MediatR;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.GetSurveyTriggerTypeByCode
{
    public sealed record GetSurveyTriggerTypeByCodeQuery(string Code) : IRequest<GetSurveyTriggerTypeByCodeResult>;
}
