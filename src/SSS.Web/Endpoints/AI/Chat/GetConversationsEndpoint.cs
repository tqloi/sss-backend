using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.Chat.GetConversations;
using SSS.Domain.Entities.AI;
using System.Security.Claims;

namespace SSS.Web.Endpoints.AI.Chat
{
    public class GetConversationsEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetConversationsQuery, IEnumerable<AiConversation>>
    {
        public override void Configure()
        {
            Get("/api/ai/chat/conversations/{RoadmapId?}");
            
        }

        public override async Task HandleAsync(GetConversationsQuery req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            req.UserId = userId!;

            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}

