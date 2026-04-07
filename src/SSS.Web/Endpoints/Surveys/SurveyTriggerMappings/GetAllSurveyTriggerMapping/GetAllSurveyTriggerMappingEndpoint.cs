using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.Surveys.GetAllSurvey;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.GetAllSurveyTriggerMapping;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.GetAllSurveyTriggerMapping
{
    public class GetAllSurveyTriggerMappingEndpoint(ISender sender): Endpoint<GetAllSurveyTriggerMappingQuery, GetAllSurveyTriggerMappingResult>
    {
        public override void Configure()
        {
            Get("/api/surveys/surveytriggermapping/all");
            Description(d => d.WithTags("SurveyTriggerMappings"));
            Summary(s => s.Summary = "Get all SurveyTriggerMappings (paginated)");
            Roles("Analyst");
        }

        public override async Task HandleAsync(GetAllSurveyTriggerMappingQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);
        }
    }
}
