using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.CreateAiQuizQuestions;

namespace SSS.Web.Endpoints.AI.CreateAiQuizQuestions
{
    public class CreateAiQuizQuestionsEndpoint(ISender sender)
        : Endpoint<CreateAiQuizQuestionsCommand, CreateAiQuizQuestionsResult>
    {
        public override void Configure()
        {
            Post("/api/ai/create-quiz-questions");
        }

        public override async Task HandleAsync(CreateAiQuizQuestionsCommand req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
