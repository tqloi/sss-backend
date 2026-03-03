using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestionOptions.Common
{
    public class QuizQuestionOptionDto
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public string ValueKey { get; set; } = null!;

        public string DisplayText { get; set; } = null!;

        public bool IsCorrect { get; set; }

        public decimal? ScoreValue { get; set; }

        public int OrderNo { get; set; }
    }
}
