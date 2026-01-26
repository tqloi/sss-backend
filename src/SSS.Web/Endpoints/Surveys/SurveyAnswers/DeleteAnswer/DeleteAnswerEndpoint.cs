using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyAnswers.DeleteAnswer;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.DeleteAnswer
{
    public class DeleteAnswerEndpoint(ISender sender) 
        : EndpointWithoutRequest<DeleteAnswerResponse>
    {
        public override void Configure()
        {
            Delete("/api/surveys/responses/answers/{answerId}");
            Description(d => d.WithTags("SurveyAnswers"));
            Summary(s => s.Summary = "Delete a survey answer by ID");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var answerId = Route<long>("answerId");
            var command = new DeleteAnswerCommand(answerId);

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}