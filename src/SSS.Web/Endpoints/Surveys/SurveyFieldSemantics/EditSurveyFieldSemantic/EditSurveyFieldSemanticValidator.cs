using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.EditSurveyFieldSemantic;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyFieldSemantics.EditSurveyFieldSemantic
{
    public class EditSurveyFieldSemanticValidator : Validator<EditSurveyFieldSemanticCommand>
    {
        public EditSurveyFieldSemanticValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");
            RuleFor(x => x.SurveyQuestionId)
                .NotEmpty().WithMessage("SurveyQuestionId is required")
                .GreaterThan(0).WithMessage("FieldName cannot exceed 100 characters");
            RuleFor(x => x.DimensionCode)
                .NotEmpty().WithMessage("SemanticType is required")
                .MaximumLength(100).WithMessage("DimensionCode cannot exceed 100 characters");

            RuleFor(x => x.Evaluates)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
            RuleFor(x => x.AIHint)
                .MaximumLength(1000).WithMessage("AIHint cannot exceed 100 characters");
        }
    }
}
