using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.Common
{
    public sealed class RoadmapDto
    {
        public RoadmapMetaDto Roadmap { get; init; } = null!;
        public IReadOnlyList<RoadmapNodeDto> Nodes { get; init; } = [];
        public IReadOnlyList<RoadmapEdgeDto> Edges { get; init; } = [];
    }
}
