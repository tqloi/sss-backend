using MediatR;
using SSS.Application.Features.Content.LearningSubject.Create;
using FastEndpoints;

namespace SSS.Web.Endpoints.Content.LearningSubject.Create
{
    public class CreateLearningSubjectEndpoint(ISender sender)
        : Endpoint<CreateLearningSubjectCommand, CreateLearningSubjectResult>
    {
        public override void Configure()
        {
            Post("/api/learning-subjects");
            Summary(s => s.Summary = "Create a new learning subject");
            Description(d => d.WithTags("LearningSubjects"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            CreateLearningSubjectCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);

            if (!result.Success)
            {
                await SendAsync(result, statusCode: 400, ct);
                return;
            }

            await SendOkAsync(result, ct);
        }
    }
}
