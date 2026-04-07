using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyResponses.SubmitResponse;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.SubmitResponse
{
    public class SubmitResponseValidator: Validator<SubmitResponseCommand>
    {
        public SubmitResponseValidator()
        {
            
            RuleFor(x => x.SubmittedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("SubmittedAt cannot be in the future.");
            
            RuleFor(x => x.TriggerReason)
                .IsInEnum().WithMessage("TriggerReason must be a valid enum value.");
        }

    }
}
