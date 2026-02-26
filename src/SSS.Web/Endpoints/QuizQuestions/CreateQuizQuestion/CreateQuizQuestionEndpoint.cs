using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestions.CreateQuizQuestion;

namespace SSS.Web.Endpoints.QuizQuestions.CreateQuizQuestion
{
    public class CreateQuizQuestionEndpoint(ISender sender)
        : Endpoint<CreateQuizQuestionCommand, CreateQuizQuestionResult>
    {
        public override void Configure()
        {
            Post("/api/quiz-question");
            Summary(s => s.Summary = "Create a new quiz question");
            Description(d => d.WithTags("QuizQuestions"));
        }
        public override async Task HandleAsync(CreateQuizQuestionCommand req, CancellationToken ct)
        => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
