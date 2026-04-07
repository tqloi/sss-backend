using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById
{
    public sealed record GetSurveyQuestionByIdQuery(long Id, long SurveyId) : IRequest<GetSurveyQuestionByIdResult>;
}
