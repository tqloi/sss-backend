using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.GetResponseById
{
    public sealed record GetResponseByIdQuery(long Id): IRequest<GetResponseByIdResult>;
    
}
