using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Quizzes.Common
{
    public class QuizDto
    {
        public long Id { get; set; }
        public long StudyPlanModuleId { get; set; } = default!;
        public string? Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public decimal? TotalScore { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
    }
}
