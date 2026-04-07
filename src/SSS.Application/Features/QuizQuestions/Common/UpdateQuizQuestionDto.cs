using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestions.Common
{
    public class UpdateQuizQuestionDto
    {
        public string Level { get; set; } = null!;

        public string QuestionKey { get; set; } = null!;

        public string Prompt { get; set; } = null!;

        public QuizQuestionType Type { get; set; }

        public decimal ScoreWeight { get; set; }

        public int OrderNo { get; set; }

        public bool IsRequired { get; set; }
    }
}
