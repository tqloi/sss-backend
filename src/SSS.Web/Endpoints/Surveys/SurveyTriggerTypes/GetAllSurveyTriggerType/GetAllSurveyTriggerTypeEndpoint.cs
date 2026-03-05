using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.GetAllSurveyTriggerTypes;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.GetAllSurveyTriggerType
{
    public class GetAllSurveyTriggerTypeEndpoint(ISender sender)
        : EndpointWithoutRequest<GetAllSurveyTriggerTypeResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/surveytriggertype/all");
            Description(d => d.WithTags("SurveyTriggerTypes"));
            Summary(s => s.Summary = "Get all active SurveyTriggerTypes");
            Roles("Analyst");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var result = await sender.Send(new GetAllSurveyTriggerTypeQuery(), ct);
            await SendOkAsync(result, ct);
        }
    }
}
