using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Quizzes.Common
{
    public sealed record CreateQuizDto
    (
        long RoadmapNodeId,
        string? Title,
        string? Description,
        decimal? TotalScore,
        string Level,
        decimal PassingScore
    );
}
