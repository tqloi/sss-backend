using MediatR;
using SSS.Application.Features.Surveys.Surveys.GetAllSurvey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.GetAllSurveyTriggerMapping
{
    public sealed record GetAllSurveyTriggerMappingQuery
     (
            int PageIndex,
            int PageSize
        ) : IRequest<GetAllSurveyTriggerMappingResult>;
}
