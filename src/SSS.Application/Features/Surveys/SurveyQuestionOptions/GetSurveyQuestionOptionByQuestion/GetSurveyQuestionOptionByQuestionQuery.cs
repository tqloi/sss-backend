using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionByQuestion
{
    public sealed record GetSurveyQuestionOptionByQuestionQuery
    (
        long QuestionId,
        //string? SearchWord,
        int PageIndex,
        int PageSize
        ) : IRequest<GetSurveyQuestionOptionByQuestionResult>;
    
}
