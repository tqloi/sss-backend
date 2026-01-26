using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Quizzes.Common
{
    public class UpdateQuizDto
    {
        public long StudyPlanModuleId { get; set; } = default!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? TotalScore { get; set; }
    }
}
