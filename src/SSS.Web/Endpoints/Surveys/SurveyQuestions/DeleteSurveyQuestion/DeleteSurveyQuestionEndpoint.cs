using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestions.DeleteSurveyQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.DeleteSurveyQuestion
{
    public class DeleteSurveyQuestionEndpoint(ISender sender): EndpointWithoutRequest<DeleteSurveyQuestionResponse>
    {
        public override void Configure()
        {
            Delete("api/surveys/question/{id}");
            Summary(s =>
            {
                s.Summary = "Delete a survey question by id";
            });
            Roles("Admin");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var id = Route<int>("id");
            var response = await sender.Send(new DeleteSurveyQuestionCommand(id), ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
