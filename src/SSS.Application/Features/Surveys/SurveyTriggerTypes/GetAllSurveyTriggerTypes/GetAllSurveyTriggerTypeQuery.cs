using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.GetAllSurveyTriggerTypes
{
    public sealed record GetAllSurveyTriggerTypeQuery() : IRequest<GetAllSurveyTriggerTypeResult>;
}
