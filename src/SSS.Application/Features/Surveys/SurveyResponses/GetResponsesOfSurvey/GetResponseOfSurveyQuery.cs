using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.GetResponsesOfSurvey
{
    public sealed record GetResponseOfSurveyQuery
    (
        long surveyId,
        //string? SearchWord,
        int PageIndex,
        int PageSize
        ) : IRequest<GetResponseOfSurveyResult>;
}
