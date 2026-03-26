using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.GetActiveSession
{
    public class GetActiveSessionQuery : IRequest<GetActiveSessionResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public long? PlanId { get; set; }
    }
}
