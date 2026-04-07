using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.GetResponsesOfSurvey
{
    public sealed record GetResponseOfSurveyResult(
            bool Success,
            string Message,
            PaginatedList<SurveyResponseDto> Data = null) : GenericResponseRecord<PaginatedList<SurveyResponseDto>>(Success, Message, Data);
}
