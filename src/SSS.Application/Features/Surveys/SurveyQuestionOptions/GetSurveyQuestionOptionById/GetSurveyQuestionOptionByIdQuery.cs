using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionById
{
    public sealed record GetSurveyQuestionOptionByIdQuery(long Id): IRequest<GetSurveyQuestionOptionByIdResult>;

}
