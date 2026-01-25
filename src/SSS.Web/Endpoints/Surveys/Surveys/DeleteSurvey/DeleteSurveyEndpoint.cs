using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.Surveys.DeleteSurvey;

namespace SSS.Web.Endpoints.Surveys.Surveys.DeleteSurvey
{
    public class DeleteSurveyEndpoint(ISender sender): EndpointWithoutRequest<DeleteSurveyResponse>
    {
        public override void Configure()
        {
            Delete("api/surveys/{id}");
            Summary(s =>
            {
                s.Summary = "Delete a survey by id";
            });
            Roles("Admin");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");
            var response = await sender.Send(new DeleteSurveyCommand(id), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
