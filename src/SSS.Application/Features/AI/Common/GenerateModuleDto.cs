using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.Common
{
    public sealed class GenerateModuleDto
    {
        public RoadmapMetaDto Roadmap { get; init; } = null!;
        public RoadmapNodeDto TargetNode { get; init; } = null!;
        public IReadOnlyList<RoadmapNodeDto> PrerequisiteNodes { get; init; } = [];
    }
}
