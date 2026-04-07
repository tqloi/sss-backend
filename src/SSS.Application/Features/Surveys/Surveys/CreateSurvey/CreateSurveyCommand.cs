using MediatR;
using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Surveys.CreateSurvey
{
    public sealed record CreateSurveyCommand(
        
        string? Title,
        string Code,
        SurveyStatus Status 
        ): IRequest<CreateSurveyResponse>;
    
        
    
}
