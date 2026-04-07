using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.EditSurvey
{
    public sealed record EditSurveyResponse
    (bool Success,
            string Message,
            SurveyDto? Data = null) : GenericResponseRecord<SurveyDto>(Success, Message, Data);
        
}
