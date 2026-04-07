using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.Surveys.GetAllSurvey;

namespace SSS.Web.Endpoints.Surveys.Surveys.GetAllSurveys
{
    public class GetAllSurveyValidator : Validator<GetAllSurveyQuery>
    {
        public GetAllSurveyValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0).WithMessage("PageIndex must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
        }
    }
}
