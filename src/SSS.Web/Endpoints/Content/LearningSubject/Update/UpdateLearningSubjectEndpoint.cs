using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.LearningSubject.Update;

namespace SSS.Web.Endpoints.Content.LearningSubject.Update
{
    public class UpdateLearningSubjectEndpoint(ISender sender)
        : Endpoint<UpdateLearningSubjectCommand, UpdateLearningSubjectResult>
    {
        public override void Configure()
        {
            Put("/api/learning-subjects/{id}");
            Summary(s => s.Summary = "Update an existing learning subject");
            Description(d => d.WithTags("LearningSubjects"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            UpdateLearningSubjectCommand req,
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
