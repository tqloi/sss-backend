using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Common
{
    public sealed record SurveyDto
    (
       
        string? Title,
        string Code  = null!,
        SurveyStatus Status  = SurveyStatus.Draft
    );
}
