using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAttempts.Common
{
    public class QuizAttemptDto
    {
        public long Id { get; set; }

        public long QuizId { get; set; }

        public string UserId { get; set; } = null!;

        public DateTime StartedAt { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public decimal? Score { get; set; }

        public QuizAttemptStatus Status { get; set; }
    }
}
