using FastEndpoints;
using MediatR;
using SSS.Application.Features.Surveys.SurveyAnswers.UpdateAnswer;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.UpdateAnswer
{
    public class UpdateAnswerEndpoint(ISender sender) 
        : Endpoint<UpdateAnswerCommand, UpdateAnswerResponse>
    {
        public override void Configure()
        {
            Put("/api/surveys/responses/answers/{answerId}");
            Description(d => d.WithTags("Survey Answers"));
            Summary(s => s.Summary = "Update a survey answer");
        }

        public override async Task HandleAsync(UpdateAnswerCommand req, CancellationToken ct)
        {
            var answerId = Route<long>("answerId");
            var command = req with { AnswerId = answerId };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}