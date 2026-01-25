using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.CreateSurveyQuestion
{
    public sealed record CreateSurveyQuestionResponse(
            bool Success,
            string Message,
            SurveyQuestionDto? Data = null) : GenericResponseRecord<SurveyQuestionDto>(Success, Message, Data);
}
