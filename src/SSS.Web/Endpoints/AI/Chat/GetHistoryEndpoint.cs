using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.Chat.GetHistory;
using SSS.Domain.Entities.AI;

namespace SSS.Web.Endpoints.AI.Chat
{
    public class GetHistoryEndpoint(ISender sender)
        : Endpoint<GetHistoryQuery, IEnumerable<AiChatMessage>>
    {
        public override void Configure()
        {
            Get("ai/chat/{ConversationId}/history");
            
        }

        public override async Task HandleAsync(GetHistoryQuery req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
