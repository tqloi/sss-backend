using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.GetSurveyTriggerTypeByCode;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.GetSurveyTriggerTypeByCode
{
    public class GetSurveyTriggerTypeByCodeEndpoint(ISender sender)
        : EndpointWithoutRequest<GetSurveyTriggerTypeByCodeResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/surveytriggertype/{code}");
            Description(d => d.WithTags("SurveyTriggerTypes"));
            Summary(s => s.Summary = "Get a SurveyTriggerType by code");
            Roles("Analyst");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var code = Route<string>("code");
            var response = await sender.Send(new GetSurveyTriggerTypeByCodeQuery(code!), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
