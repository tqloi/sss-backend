using MediatR;
using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyQuestions.CreateSurveyQuestion
{
    public sealed record CreateSurveyQuestionCommand
    (
        long SurveyId,
        string QuestionKey,
        string Prompt,
        SurveyQuestionType Type,
        int OrderNo,
        bool IsRequired,
        int? ScaleMin,
        int? ScaleMax
    ): IRequest<CreateSurveyQuestionResponse>;
}
