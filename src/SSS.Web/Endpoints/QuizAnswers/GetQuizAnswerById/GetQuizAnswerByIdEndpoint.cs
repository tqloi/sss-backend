using FastEndpoints;
using MediatR;
using SSS.Application.Features.QuizAnswers.GetQuizAnswerById;

namespace SSS.Web.Endpoints.QuizAnswers.GetQuizAnswerById
{
    public class GetQuizAnswerByIdEndpoint(ISender sender)
        : Endpoint<GetQuizAnswerByIdQuery, GetQuizAnswerByIdResult>
    {
        public override void Configure()
        {
            Get("/api/quiz-answer/{id}");
            Summary(s => s.Summary = "Get a quiz answer by ID");
            Description(d => d.WithTags("QuizAnswers"));
        }
        public override async Task HandleAsync(GetQuizAnswerByIdQuery req, CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            if (result.QuizAnswerDto == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }
            await SendOkAsync(result, ct);
        }
    }
}
