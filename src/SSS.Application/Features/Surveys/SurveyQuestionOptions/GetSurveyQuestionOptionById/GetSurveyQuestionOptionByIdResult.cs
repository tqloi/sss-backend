using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionById
{
    public sealed record GetSurveyQuestionOptionByIdResult
    (bool Success,
             string Message,
             SurveyQuestionOptionDto? Data = null) : GenericResponseRecord<SurveyQuestionOptionDto>(Success, Message, Data);
}
