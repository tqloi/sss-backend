using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiTaskItems
{
    public sealed record CreateAiTaskItemsCommand
    (
        string userId,
        long roadmapId
    )
    : IRequest<CreateAiTaskItemsResult>;
}
