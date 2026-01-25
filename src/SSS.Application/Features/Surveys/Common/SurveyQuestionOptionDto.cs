using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Common
{
    public sealed record SurveyQuestionOptionDto
    (
     long QuestionId,
     string ValueKey,
     string DisplayText,
     decimal? Weight,
     int OrderNo, 
     bool AllowFreeText 
    );
    
}
