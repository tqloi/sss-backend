using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.GetSurveyTriggerMappingById;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.GetSurveyTriggerMappingById
{
    public class GetSurveyTriggerMappingByIdEndpoint(ISender sender): EndpointWithoutRequest<GetSurveyTriggerMappingByIdResult>
    {
        public override void Configure()
        {
            Get("api/surveys/surveytriggermapping/{id}");
            Description(d => d.WithTags("SurveyTriggerMappings"));
            Summary(s =>
            {
                s.Summary = "Get a survey trigger mapping by id";
            });
            Roles("Analyst");
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");
            var response = await sender.Send(new GetSurveyTriggerMappingByIdQuery(id), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
