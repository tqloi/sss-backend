using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyResponses.GetResponseById;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.GetResponseById
{
    public class GetResponseByIdValidator : Validator<GetResponseByIdQuery>
    {
        public GetResponseByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
