using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyFieldSemantics.GetSurveyFieldSemanticByQuestion
{
    public sealed record GetSurveyFieldSemanticByQuestionQuery
    (long QuestionId
    ): IRequest<GetSurveyFieldSemanticByQuestionResult>;
}
