using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyResponses.DeleteResponse;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.DeleteResponse
{
    public class DeleteResponseValidator: Validator<DeleteResponseCommand>
    {
        public DeleteResponseValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");
        }
    }
}
