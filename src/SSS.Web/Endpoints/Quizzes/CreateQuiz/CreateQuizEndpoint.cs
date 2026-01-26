using FastEndpoints;
using MediatR;
using SSS.Application.Features.Quizzes.CreateQuiz;

namespace SSS.Web.Endpoints.Quizzes.CreateQuiz
{
    public sealed class CreateQuizEndpoint(ISender sender)
        : Endpoint<CreateQuizCommand, CreateQuizResult>
    {
        public override void Configure()
        {
            Post("/api/quiz");
            Summary(s => s.Summary = "Create a new quiz");
            Description(d => d.WithTags("Quizzes"));
            Roles("Admin");
        }
        public override async Task HandleAsync(CreateQuizCommand req, CancellationToken ct)
        => await SendAsync(await sender.Send(req, ct), cancellation: ct);       
    }
}
