using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.GetSurveyTriggerMappingById
{
    public sealed record GetSurveyTriggerMappingByIdQuery(long Id): IRequest<GetSurveyTriggerMappingByIdResult>
    {
    }
}
