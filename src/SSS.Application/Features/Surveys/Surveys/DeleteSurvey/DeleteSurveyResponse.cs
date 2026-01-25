using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.DeleteSurvey
{
    public sealed record DeleteSurveyResponse(
        bool Success,
        string Message,
        SurveyDto? Data = null) : GenericResponseRecord<SurveyDto>(Success, Message, Data);
}
