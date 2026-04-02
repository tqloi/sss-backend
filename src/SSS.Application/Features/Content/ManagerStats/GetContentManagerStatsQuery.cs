using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Content.ManagerStats
{
    public sealed class GetContentManagerStatsQuery : IRequest<GetContentManagerStatsResult>
    {
        public long? SubjectId { get; set; }

        [JsonIgnore]
        public string ManagerId { get; set; } = null!;
    }
}
