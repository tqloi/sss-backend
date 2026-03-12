using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.GetRecentSessions
{
    public class GetRecentSessionsQuery : IRequest<GetRecentSessionsResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public int Limit { get; set; } = 5;
    }
}
