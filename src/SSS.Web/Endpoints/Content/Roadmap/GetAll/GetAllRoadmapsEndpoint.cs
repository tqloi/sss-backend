using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.Roadmap.GetAll;

namespace SSS.Web.Endpoints.Content.Roadmap.GetAll
{
    public class GetAllRoadmapsEndpoint(ISender sender)
        : Endpoint<GetAllRoadmapsQuery, GetAllRoadmapsResult>
    {
        public override void Configure()
        {
            Get("/api/roadmaps");
            Summary(s => s.Summary = "Get all roadmaps with pagination and filtering");
            Description(d => d.WithTags("Roadmaps"));
            Roles("Admin");
        }

        public override async Task HandleAsync(
            GetAllRoadmapsQuery req,
            CancellationToken ct)
        {
            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);
        }
    }
}
