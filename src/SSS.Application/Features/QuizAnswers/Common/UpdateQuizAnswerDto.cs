using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAnswers.Common
{
    public class UpdateQuizAnswerDto
    {
        public long QuestionId { get; set; }
        public long? OptionId { get; set; }
        public string? TextValue { get; set; }
        public decimal? NumberValue { get; set; }
    }
}
