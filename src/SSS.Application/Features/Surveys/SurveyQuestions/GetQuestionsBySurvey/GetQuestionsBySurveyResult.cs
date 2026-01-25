using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey
{
    public sealed record GetQuestionsBySurveyResult(
            bool Success,
            string Message,
            PaginatedList<SurveyQuestionDto> Data = null) : GenericResponseRecord<PaginatedList<SurveyQuestionDto>>(Success, Message, Data);
    //public sealed record GetQuestionBySurveyResponse(PaginatedResponse<SurveyQuestionDto> Data);
}
