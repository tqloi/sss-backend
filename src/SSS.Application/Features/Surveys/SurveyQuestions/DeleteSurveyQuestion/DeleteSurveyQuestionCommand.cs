using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.DeleteSurveyQuestion
{
    public sealed record DeleteSurveyQuestionCommand(long Id): IRequest<DeleteSurveyQuestionResponse>;
    
}
