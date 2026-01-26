using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyResponses.DeleteResponse;
using SSS.Application.Features.Surveys.Surveys.DeleteSurvey;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.DeleteResponse
{
    public class DeleteResponseEndpoint(ISender sender): EndpointWithoutRequest<DeleteResponseResponse>
    {
        public override void Configure()
        {
            Delete("/api/surveys/responses/{responseId}");
            Description(d => d.WithTags("SurveyResponses"));
            Summary(s => s.Summary = "Delete a survey response");
        }
        public override async Task HandleAsync(CancellationToken ct)
        {
            var Id = Route<long>("responseId");

            var response = await sender.Send(new DeleteResponseCommand(Id), ct);
            await SendAsync(response, cancellation: ct);
            
        }

    }
}
