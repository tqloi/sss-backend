using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.Surveys.CreateSurvey;

namespace SSS.Web.Endpoints.Surveys.Surveys.CreateSurvey
{
    public class CreateSurveyEndpoint(ISender sender): Endpoint<CreateSurveyCommand, CreateSurveyResponse>
    {
        public override void Configure()
        {
            Post("api/surveys");
            Summary(s => s.Summary = "Create new survey");
            Roles("Admin");
        }

        public override async Task HandleAsync(CreateSurveyCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
