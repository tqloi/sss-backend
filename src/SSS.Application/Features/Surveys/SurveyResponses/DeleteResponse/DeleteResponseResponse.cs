using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.DeleteResponse
{
    public sealed record DeleteResponseResponse(
        bool Success,
        string Message,
        SurveyResponseDto? Data = null) : GenericResponseRecord<SurveyResponseDto>(Success, Message, Data);
}
