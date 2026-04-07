using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.GetAllSurveyTriggerTypes
{
    public sealed record GetAllSurveyTriggerTypeResult(
        List<SurveyTriggerTypeDto> SurveyTriggerTypes
    );
}
