using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestions.CreateQuizQuestions;

namespace SSS.Web.Endpoints.QuizQuestions.CreateQuizQuestions
{
    public class CreateQuizQuestionsEndpoint(ISender sender)
        : Endpoint<CreateQuizQuestionsCommand, CreateQuizQuestionsResult>
    {
        public override void Configure()
        {
            Post("/api/quiz-questions");
            Summary(s => s.Summary = "Create multiple quiz questions");
            Description(d => d.WithTags("QuizQuestions"));
            Roles("ContentManager");
        }

        public override async Task HandleAsync(CreateQuizQuestionsCommand req, CancellationToken ct)
            => await SendAsync(await sender.Send(req, ct), cancellation: ct);
    }
}
