using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.DeleteSurvey
{
    public sealed record DeleteSurveyCommand(long Id): IRequest<DeleteSurveyResponse>;
    
        
    
}
