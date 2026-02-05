using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiAddVecDb
{
    public class CreateAiAddVecDbCommand : IRequest<CreateAiAddVecDbResponse>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public string StudyPlanId { get; set; }
    }
}
