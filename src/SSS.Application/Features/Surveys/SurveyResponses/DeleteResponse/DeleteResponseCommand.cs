using MediatR;
using SSS.Application.Features.Surveys.Surveys.DeleteSurvey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.DeleteResponse
{
    public sealed record DeleteResponseCommand(long Id) : IRequest<DeleteResponseResponse>;
    
}
