using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestionOptions.CreateQuizQuestionOption;

namespace SSS.Web.Endpoints.QuizQuestionOptions.CreateQuizQuestionOption
{
    public class CreateQuizQuestionOptionEndpoint(ISender sender)
        : Endpoint<CreateQuizQuestionOptionCommand, CreateQuizQuestionOptionResult>
    {
        public override void Configure()
        {
            Post("/api/quiz-question-options");
            Summary(s => s.Summary = "Create a new quiz question option");
            Description(s => s.WithDescription( "Creates a new quiz question option with the provided details."));
            AllowAnonymous();
        }
        public override async Task HandleAsync(CreateQuizQuestionOptionCommand req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result);
        }
    }
}
