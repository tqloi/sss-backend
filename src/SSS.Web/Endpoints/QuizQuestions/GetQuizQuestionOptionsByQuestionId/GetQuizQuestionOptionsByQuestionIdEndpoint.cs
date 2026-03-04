using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestionOptions.GetQuizQuestionOptionsByQuestionId;

namespace SSS.Web.Endpoints.QuizQuestions.GetQuizQuestionOptionsByQuestionId
{
    public class GetQuizQuestionOptionsByQuestionIdEndpoint(ISender sender)
        : Endpoint<GetQuizQuestionOptionsByQuestionIdQuery, GetQuizQuestionOptionsByQuestionIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-question/{questionId}/options");
            Summary(s => s.Summary = "Get a quiz question by id");
            Description(d => d.WithTags("QuizQuestions"));
        }
        public override async Task HandleAsync(GetQuizQuestionOptionsByQuestionIdQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendAsync(result);
        }

    }
}
