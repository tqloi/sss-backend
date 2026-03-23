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
            Post("ai/create-quiz-questions");
            Description(d => d.WithTags("AI"));

        }

        public override async Task HandleAsync(CreateAiQuizQuestionsCommand req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
