using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.Chat.CreateConversation;
using System.Security.Claims;

namespace SSS.Web.Endpoints.AI.Chat
{
    public class CreateConversationEndpoint : Endpoint<CreateConversationCommand, CreateConversationResult>
    {
        private readonly ISender _sender;

        public CreateConversationEndpoint(ISender sender)
        {
            _sender = sender;
        }

        public override void Configure()
        {
            Post("ai/chat/conversations");
            Summary(s =>
            {
                s.Summary = "Create a new conversation for a roadmap";
                s.Description = "Creates a new AI chat conversation linked to a specific roadmap.";
            });
        }

        public override async Task HandleAsync(CreateConversationCommand req, CancellationToken ct)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            req.UserId = userId;

            var result = await _sender.Send(req, ct);
            await SendAsync(result, cancellation: ct);
        }
    }
}
