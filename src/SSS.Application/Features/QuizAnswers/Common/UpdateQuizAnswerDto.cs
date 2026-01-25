using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAnswers.Common
{
    public sealed record UpdateQuizAnswerDto
    (
        long? OptionId,
        string? TextValue,
        decimal? NumberValue,
        decimal? ScoredValue,
        long? QuestionId,
        long? AttemptId
    );
}
