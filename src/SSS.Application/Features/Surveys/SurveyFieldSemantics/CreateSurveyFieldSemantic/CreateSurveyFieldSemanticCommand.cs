using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyFieldSemantics.CreateSurveyFieldSemantic
{
    public sealed record CreateSurveyFieldSemanticCommand: IRequest<CreateSurveyFieldSemanticResponse>
    {
        public long SurveyQuestionId { get; set; }

        public string DimensionCode { get; set; } = default!;

        public string Evaluates { get; set; } = default!;

        public string? AIHint { get; set; }

        public double? Weight { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
