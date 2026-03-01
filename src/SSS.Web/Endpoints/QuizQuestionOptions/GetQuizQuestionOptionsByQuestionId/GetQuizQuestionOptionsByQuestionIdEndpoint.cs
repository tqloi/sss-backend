using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizQuestionOptions.GetQuizQuestionOptionsByQuestionId;

namespace SSS.Web.Endpoints.QuizQuestionOptions.GetQuizQuestionOptionsByQuestionId
{
    public class GetQuizQuestionOptionsByQuestionIdEndpoint(ISender sender)
        : Endpoint<GetQuizQuestionOptionsByQuestionIdQuery, GetQuizQuestionOptionsByQuestionIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-question-options/by-question-id/{QuestionId}");
            Summary(s =>
            {
                s.Summary = "Get quiz question options by question id.";
                s.Description = "Get quiz question options by question id.";
            });
            AllowAnonymous();
        }
        public override async Task HandleAsync(GetQuizQuestionOptionsByQuestionIdQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);

        }
    }
}
