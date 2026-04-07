using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.CreateSurveyTriggerMapping;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.CreateSurveyTriggerMapping
{
    public class CreateSurveyTriggerMappingEndpoint(ISender sender): Endpoint<CreateSurveyTriggerMappingCommand, CreateSurveyTriggerMappingResponse>
    {
        public override void Configure()
        {
            Post("/api/surveys/surveytriggermapping");
            Description(d => d.WithTags("SurveyTriggerMappings"));
            Summary(s => s.Summary = "Create new survey trigger mapping");
            Roles("Analyst");
        }
        public override async Task HandleAsync(CreateSurveyTriggerMappingCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
