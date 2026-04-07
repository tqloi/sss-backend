using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.GetSessionStatistics
{
    public class GetSessionStatisticsQuery : IRequest<GetSessionStatisticsResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public string? Period { get; set; } // week, month, all
    }
}
