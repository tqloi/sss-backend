using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey
{
    public sealed record GetQuestionsBySurveyQuery
        (
        long surveyId,
        //string? SearchWord,
        int PageIndex,
        int PageSize
        ) : IRequest<GetQuestionsBySurveyResult>;
    
}
