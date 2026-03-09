using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.DeleteSurveyTriggerType;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.DeleteSurveyTriggerType
{
    public class DeleteSurveyTriggerTypeEndpoint(ISender sender)
        : EndpointWithoutRequest<DeleteSurveyTriggerTypeResponse>
    {
        public override void Configure()
        {
            Delete("/api/surveys/surveytriggertype/{code}");
            Description(d => d.WithTags("SurveyTriggerTypes"));
            Summary(s => s.Summary = "Delete a SurveyTriggerType by code");
            Roles("Analyst");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var code = Route<string>("code");
            var response = await sender.Send(new DeleteSurveyTriggerTypeCommand(code!), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
