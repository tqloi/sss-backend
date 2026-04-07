using MediatR;
using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.EditSurvey
{
    public sealed record EditSurveyCommand
    (
        long Id,
        string? Title,
        string Code = null!,
        SurveyStatus Status = SurveyStatus.Draft

        ):IRequest<EditSurveyResponse>;
   
}
