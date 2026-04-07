using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.Common
{
    public class UserLearningTargetDto
    {
        public string TargetRole { get; set; } = default!;
        public string CurrentLevel { get; set; } = default!;
        public int? TargetDeadlineMonths { get; set; }

        public string? GoalDescription { get; set; }
    }
}
