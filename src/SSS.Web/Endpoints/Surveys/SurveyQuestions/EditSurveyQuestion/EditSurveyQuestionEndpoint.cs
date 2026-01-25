using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestions.EditSurveyQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.EditSurveyQuestion
{
    public class EditSurveyQuestionEndpoint(ISender sender): Endpoint<EditSurveyQuestionCommand, EditSurveyQuestionResponse>
    {
        public override void Configure()
        {
            Patch("/api/surveys/question");
            Summary(s => s.Summary = "edit a question for a survey");
            Roles("Admin");
        }
        public override async Task HandleAsync(EditSurveyQuestionCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
