using FastEndpoints;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic;

namespace SSS.Web.Endpoints.Surveys.SurveyFieldSemantics.DeleteSurveyFieldSemantic
{
    public class DeleteSurveyFieldSemanticValidator : Validator<DeleteSurveyFieldSemanticCommand>
    {
        public DeleteSurveyFieldSemanticValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Survey Field Semantic Id is required.")
                .GreaterThan(0).WithMessage("Survey Field Semantic Id must be greater than zero.");
        }
    }
}
