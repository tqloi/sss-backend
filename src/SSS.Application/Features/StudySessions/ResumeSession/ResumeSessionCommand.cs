using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.ResumeSession
{
    public class ResumeSessionCommand : IRequest<ResumeSessionResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        [JsonIgnore]
        public string SessionId { get; set; } = null!;
    }
}
