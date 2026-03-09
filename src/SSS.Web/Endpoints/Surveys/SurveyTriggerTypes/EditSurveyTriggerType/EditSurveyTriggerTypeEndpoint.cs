using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.EditSurveyTriggerType;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.EditSurveyTriggerType
{
    public class EditSurveyTriggerTypeEndpoint(ISender sender)
        : Endpoint<EditSurveyTriggerTypeCommand, EditSurveyTriggerTypeResponse>
    {
        public override void Configure()
        {
            Put("/api/surveys/surveytriggertype/edit");
            Description(d => d.WithTags("SurveyTriggerTypes"));
            Summary(s => s.Summary = "Update a SurveyTriggerType");
            Roles("Analyst");
        }

        public override async Task HandleAsync(EditSurveyTriggerTypeCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
