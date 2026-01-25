using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.Surveys.DeleteSurvey;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.Surveys.DeleteSurvey
{
    public class DeleteSurveyValidator : Validator<DeleteSurveyCommand>
    {
        public DeleteSurveyValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
