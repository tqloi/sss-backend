using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.GetAllSurvey
{
    public sealed record GetAllSurveyQuery
        (
            int PageIndex,
            int PageSize
        ) : IRequest<GetAllSurveyResult>;
    
    
}
