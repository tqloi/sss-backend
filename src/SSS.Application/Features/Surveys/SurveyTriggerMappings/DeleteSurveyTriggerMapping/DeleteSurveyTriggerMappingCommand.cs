using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.DeleteSurveyTriggerMapping
{
    public sealed record DeleteSurveyTriggerMappingCommand(long Id) : IRequest<DeleteSurveyTriggerMappingResponse>
    {
    }
}
