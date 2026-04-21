using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.Chat.DeleteConversation;
using System.Security.Claims;

namespace SSS.Web.Endpoints.AI.Chat
{
    public class DeleteConversationEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<DeleteConversationCommand, DeleteConversationResult>
    {
        public override void Configure()
        {
            Delete("/api/ai/chat/conversations/{ConversationId}");
            Summary(s =>
            {
                s.Summary = "Delete a conversation";
                s.Description = "Deletes an AI chat conversation and its message history.";
            });
        }

        public override async Task HandleAsync(DeleteConversationCommand req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            req.UserId = userId;

            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}