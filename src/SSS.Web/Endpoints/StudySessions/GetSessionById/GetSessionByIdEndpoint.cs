using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.GetSessionById;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.GetSessionById
{
    public class GetSessionByIdEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : EndpointWithoutRequest<GetSessionByIdResult>
    {
        public override void Configure()
        {
            Get("/api/study-sessions/{Id}");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Get session detail by ID");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var sessionId = Route<string>("Id");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                AddError(r => r, "Id is required.");
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetSessionByIdQuery
            {
                UserId = userId!,
                SessionId = sessionId
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
