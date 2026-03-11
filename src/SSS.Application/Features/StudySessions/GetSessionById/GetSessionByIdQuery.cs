using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.GetSessionById
{
    public class GetSessionByIdQuery : IRequest<GetSessionByIdResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public string SessionId { get; set; } = null!;
    }
}
