using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic
{
    public sealed record DeleteSurveyFieldSemanticResponse
    (
            bool Success,
            string Message,
            SurveyFieldSenmaticDto? Data = null) : GenericResponseRecord<SurveyFieldSenmaticDto>(Success, Message, Data);
}

