using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.EditSurveyQuestionOption;
using SSS.Application.Features.Surveys.Surveys.EditSurvey;

namespace SSS.Web.Endpoints.Surveys.Surveys.EditSurvey
{
    public class EditSurveyEndpoint(ISender sender): Endpoint<EditSurveyCommand, EditSurveyResponse>
    {
        public override void Configure()
        {
            Patch("/api/surveys/edit");
            Summary(s => s.Summary = "edit a survey");
            Roles("Admin");
        }
        public override async Task HandleAsync(EditSurveyCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
