using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAnswers.UpdateQuizAnswer;

namespace SSS.Web.Endpoints.QuizAnswers.UpdateQuizAnswer
{
    public class UpdateQuizAnswerEndpoint(ISender sender)
        : Endpoint<UpdateQuizAnswerCommand, UpdateQuizAnswerResult>
    {
        public override void Configure()
        {
            Put("/api/quiz-answer/{id}");
            Summary(s => s.Summary = "Update quiz answer by ID");
            Description(d => d.WithTags("QuizAnswers"));
            AllowAnonymous();
        }
        public override async Task HandleAsync(UpdateQuizAnswerCommand req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result);
        }
    }
}
