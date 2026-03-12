using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.GetSessionById;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.GetSessionById
{
    public class GetSessionByIdRequest
    {
        public string Id { get; set; } = null!;
    }

    public class GetSessionByIdEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetSessionByIdRequest, GetSessionByIdResult>
    {
        public override void Configure()
        {
            Get("/api/study-sessions/{Id}");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Get session detail by ID");
        }

        public override async Task HandleAsync(GetSessionByIdRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetSessionByIdQuery
            {
                UserId = userId!,
                SessionId = req.Id
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
