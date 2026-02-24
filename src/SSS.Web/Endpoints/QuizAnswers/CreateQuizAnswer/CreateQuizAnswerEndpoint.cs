using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAnswers.CreateQuizAnswer;

namespace SSS.Web.Endpoints.QuizAnswers.CreateQuizAnswer
{
    public class CreateQuizAnswerEndpoint(ISender sender)
        : Endpoint<CreateQuizAnswerCommand, CreateQuizAnswerResult>
    {
        public override void Configure()
        {
            Post("/api/quizanswer");
            Summary(s => s.Summary = "Create a new quiz answer");
            Description(d => d.WithTags("Quiz Answers"));
        }
        public override async Task HandleAsync(CreateQuizAnswerCommand req, CancellationToken ct)
        => await SendAsync(await sender.Send(req, ct), cancellation: ct);

    }
}
