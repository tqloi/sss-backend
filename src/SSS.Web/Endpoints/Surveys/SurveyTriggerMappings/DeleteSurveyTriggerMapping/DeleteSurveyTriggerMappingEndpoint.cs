using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.DeleteSurveyTriggerMapping;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.DeleteSurveyTriggerMapping
{
    public class DeleteSurveyTriggerMappingEndpoint(ISender sender): EndpointWithoutRequest<DeleteSurveyTriggerMappingResponse>
    {
        public override void Configure()
        {
            Delete("/api/surveys/surveytriggermapping/{id}");
            Description(d => d.WithTags("SurveyTriggerMappings"));
            Summary(s =>
            {
                s.Summary = "Delete a Survey Trigger Mapping  by id";
            });
            Roles("Analyst");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");
            var response = await sender.Send(new DeleteSurveyTriggerMappingCommand(id), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
