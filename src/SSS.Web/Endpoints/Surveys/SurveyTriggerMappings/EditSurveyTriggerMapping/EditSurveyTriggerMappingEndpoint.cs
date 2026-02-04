using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.EditSurveyTriggerMapping;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.EditSurveyTriggerMapping
{
    public class EditSurveyTriggerMappingEndpoint(ISender sender): Endpoint<EditSurveyTriggerMappingCommand, EditSurveyTriggerMappingResponse>
    {
        public override void Configure()
        {
            Patch("/api/surveys/surveytriggermapping");
            Description(d => d.WithTags("SurveyTriggerMappings"));
            Summary(s => s.Summary = "Edit a Survey Trigger Mapping");
            Roles("Analyst");
        }
        public override async Task HandleAsync(EditSurveyTriggerMappingCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    
    }
}
