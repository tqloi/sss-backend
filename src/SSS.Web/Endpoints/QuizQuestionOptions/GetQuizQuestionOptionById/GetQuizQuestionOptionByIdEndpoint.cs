using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestionOptions.GetQuizQuestionOptionById;

namespace SSS.Web.Endpoints.QuizQuestionOptions.GetQuizQuestionOptionById
{
    public class GetQuizQuestionOptionByIdEndpoint(ISender sender)
        : Endpoint<GetQuizQuestionOptionByIdQuery, GetQuizQuestionOptionByIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-question-options/{Id}");
            Summary(s => s.Summary = "Get a quiz question option by ID");
            Description(s => s.WithDescription("Retrieves a quiz question option based on the provided ID."));
            AllowAnonymous();
        }
        public override async Task HandleAsync(GetQuizQuestionOptionByIdQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result);
        }
    }
}
