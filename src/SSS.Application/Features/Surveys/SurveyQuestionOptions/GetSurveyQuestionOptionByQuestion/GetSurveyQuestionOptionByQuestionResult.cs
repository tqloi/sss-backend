using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionByQuestion
{
    public sealed record GetSurveyQuestionOptionByQuestionResult
    (
        bool Success,
        string Message,
        PaginatedList<SurveyQuestionOptionDto> Data = null) : GenericResponseRecord<PaginatedList<SurveyQuestionOptionDto>>(Success, Message, Data);
        
    
}
