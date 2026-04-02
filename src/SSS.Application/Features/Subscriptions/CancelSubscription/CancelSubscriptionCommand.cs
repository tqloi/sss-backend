using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Subscriptions.CancelSubscription;

public sealed class CancelSubscriptionCommand : IRequest<bool>
{
    [JsonIgnore]
    public string UserId { get; set; } = null!;
}
