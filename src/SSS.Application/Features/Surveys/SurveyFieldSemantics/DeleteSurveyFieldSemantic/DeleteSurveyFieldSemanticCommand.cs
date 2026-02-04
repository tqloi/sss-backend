using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic
{
    public  sealed record DeleteSurveyFieldSemanticCommand(long Id): IRequest<DeleteSurveyFieldSemanticResponse>
    {
    }
}
