using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.Common
{
    public class UserLearningBehaviorDto
    {
        // Time & availability
        public string? AvailableDaysJson { get; set; }
        public string? PreferredTimeBlocksJson { get; set; }
        public int? SessionLengthPrefMinutes { get; set; }

        // Learning style weights
        public decimal WVisual { get; set; }
        public decimal WReading { get; set; }
        public decimal WPractice { get; set; }
    }
}
