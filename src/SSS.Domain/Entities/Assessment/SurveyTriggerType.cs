using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Domain.Entities.Assessment
{
    public class SurveyTriggerType
    {
        public string Code { get; set; } = default!;        
        public string DisplayName { get; set; } = default!; 
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<SurveyTriggerMapping> TriggerMappings { get; set; } = new HashSet<SurveyTriggerMapping>();

    }
}
