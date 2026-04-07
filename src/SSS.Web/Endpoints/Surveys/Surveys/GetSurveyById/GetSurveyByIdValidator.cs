using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById;
using SSS.Application.Features.Surveys.Surveys.GetSurveyById;

namespace SSS.Web.Endpoints.Surveys.Surveys.GetSurveyById
{
    public class GetSurveyByIdValidator : Validator<GetSurveyByIdQuery>
    {
        public GetSurveyByIdValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        }


    }
}
