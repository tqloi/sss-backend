using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.PauseSession
{
    public class PauseSessionCommand : IRequest<PauseSessionResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        [JsonIgnore]
        public string SessionId { get; set; } = null!;
    }
}
