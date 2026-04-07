using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.LearningSubject.GetAll;

namespace SSS.Web.Endpoints.Content.LearningSubject.GetAll
{
    public class GetAllLearningSubjectEndpoint(ISender sender)
        : Endpoint<GetAllLearningSubjectQuery, GetAllLearningSubjectResult>
    {
        public override void Configure()
        {
            Get("/api/learning-subjects");
            Summary(s => s.Summary = "Get all learning subjects with pagination and optional category filter");
            Description(d => d.WithTags("LearningSubjects"));
            AllowAnonymous();
        }

        public override async Task HandleAsync(
            GetAllLearningSubjectQuery req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);
        }
    }
}
