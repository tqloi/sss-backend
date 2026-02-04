using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiTaskItems
{
    public class CreateAiTaskItemsCommand : IRequest<CreateAiTaskItemsResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public long studyPlanModuleId { get; set; }
    }
}
