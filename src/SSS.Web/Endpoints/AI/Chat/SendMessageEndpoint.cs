using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.Chat.SendMessage;
using System.Security.Claims;

namespace SSS.Web.Endpoints.AI.Chat
{
    public class SendMessageEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<SendMessageCommand, SendMessageResult>
    {
        public override void Configure()
        {
            Post("/api/ai/chat/send");
            
        }

        public override async Task HandleAsync(SendMessageCommand req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            req.UserId = userId!;

            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
