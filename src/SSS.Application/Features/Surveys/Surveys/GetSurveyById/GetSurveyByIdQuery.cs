using MediatR;
using SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.GetSurveyById
{
    public sealed record  GetSurveyByIdQuery(long Id) : IRequest<GetSurveyByIdResult>;
    
}
