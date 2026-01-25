using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption
{
    public sealed record CreateSurveyQuestionOptionResponse
    (
        bool Success,
        string Message,
        SurveyQuestionOptionDto? Data = null) : GenericResponseRecord<SurveyQuestionOptionDto>(Success, Message, Data);
    
}
