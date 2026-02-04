using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Common
{
    public sealed record  SurveyFieldSenmaticDto
    (
        long Id,
        long SurveyQuestionId,
        string DimensionCode,
        string Evaluates,
        string? AIHint,
        double? Weight,
        DateTime CreatedAt
);
}
