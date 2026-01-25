using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.LearningSubject.Delete;

namespace SSS.Web.Endpoints.Content.LearningSubject.Delete
{
    public class DeleteLearningSubjectEndpoint(ISender sender)
        : Endpoint<DeleteLearningSubjectCommand, DeleteLearningSubjectResult>
    {
        public override void Configure()
        {
            Delete("/api/learning-subjects/{id}");
            Summary(s => s.Summary = "Delete a learning subject");
            Roles("Admin");
        }

        public override async Task HandleAsync(
            DeleteLearningSubjectCommand req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);

            if (!result.Success)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendOkAsync(result, ct);
        }
    }
}
