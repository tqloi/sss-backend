using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiAddVecDb
{
    public sealed record CreateAiAddVecDbResponse
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
    }
}
