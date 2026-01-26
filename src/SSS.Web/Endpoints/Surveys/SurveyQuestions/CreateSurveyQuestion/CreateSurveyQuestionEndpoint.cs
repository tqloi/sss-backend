using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestions.CreateSurveyQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.CreateSurveyQuestion
{
    public class CreateSurveyQuestionEndpoint(ISender sender) : Endpoint<CreateSurveyQuestionCommand, CreateSurveyQuestionResponse>
    {
        public override void Configure()
        {
            Post("api/surveys/question");
            Description(d => d.WithTags("SurveyQuestions"));
            Summary(s => s.Summary = "Create new survey question");
            Roles("Admin");
        }

        public override async Task HandleAsync(CreateSurveyQuestionCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
