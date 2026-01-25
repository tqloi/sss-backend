using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.EditSurveyQuestionOption;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.EditSurveyQuestionOption
{
    public class EditSurveyQuestionOptionEndpoint(ISender sender): Endpoint<EditSurveyQuestionOptionCommand, EditSurveyQuestionOptionResponse>
    {
        public override void Configure()
        {
            Patch("/api/surveys/question/option");
            Description(d => d.WithTags("Survey Options"));
            Summary(s => s.Summary = "edit an option for a question");
            Roles("Admin");
        }
        public override async Task HandleAsync(EditSurveyQuestionOptionCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
