using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.CreateSurveyTriggerType;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.CreateSurveyTriggerType
{
    public class CreateSurveyTriggerTypeEndpoint(ISender sender)
        : Endpoint<CreateSurveyTriggerTypeCommand, CreateSurveyTriggerTypeResponse>
    {
        public override void Configure()
        {
            Post("/api/surveys/surveytriggertype");
            Description(d => d.WithTags("SurveyTriggerTypes"));
            Summary(s => s.Summary = "Create a new SurveyTriggerType");
            Roles("Analyst");
        }

        public override async Task HandleAsync(CreateSurveyTriggerTypeCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
