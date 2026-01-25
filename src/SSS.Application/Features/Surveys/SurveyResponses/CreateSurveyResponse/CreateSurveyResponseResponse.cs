using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse
{
    public sealed record CreateSurveyResponseResponse(
            bool Success,
            string Message,
            SurveyResponseDto? Data = null) : GenericResponseRecord<SurveyResponseDto>(Success, Message, Data);
}
