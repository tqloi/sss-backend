using FastEndpoints;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using SSS.Application.Features.Surveys.SurveyAnswers.GetAllAnswersByResponse;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.GetAllAnswersByResponse
{
    public class GetAllAnswersByResponseValidator : Validator<GetAllAnswersByResponseQuery>
    {
        public GetAllAnswersByResponseValidator()
        {
            RuleFor(x => x.ResponseId)
                .GreaterThan(0).WithMessage("ResponseId must be greater than 0");
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("PageIndex must be greater than 0");
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0");
        }
    }
}
