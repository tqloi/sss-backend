using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.Common
{
    public sealed class RoadmapNodeDto
    {
        public long Id { get; init; }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public string Difficulty { get; init; } = null!;
        public int OrderNo { get; init; }
        public IReadOnlyList<ResourceDto> Resources { get; init; } = [];
    }
}
