using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption
{
    public sealed record CreateSurveyQuestionOptionCommand
        (
         long QuestionId,
         string ValueKey,
         string DisplayText,
         decimal? Weight,
         int OrderNo,
         bool AllowFreeText
        ) :IRequest<CreateSurveyQuestionOptionResponse>;
    
}
