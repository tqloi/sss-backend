using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.Surveys.TakeSurvey;

namespace SSS.Web.Endpoints.Surveys.Surveys.TakeSurvey
{
    public class TakeSurveyValidator : Validator<TakeSurveyCommand>
    {
        public TakeSurveyValidator()
        {
            RuleFor(x => x.StartedAt)
                .NotEmpty()
                .WithMessage("StartedAt is required");

            RuleFor(x => x.Answers)
                .NotNull()
                .WithMessage("Answers list is required");

            RuleForEach(x => x.Answers)
                .ChildRules(answer =>
                {
                    answer.RuleFor(a => a.QuestionId)
                        .GreaterThan(0)
                        .WithMessage("QuestionId must be greater than 0");

                    answer.RuleFor(a => a.AnsweredAt)
                        .NotEmpty()
                        .WithMessage("AnsweredAt is required for each answer");
                });

            RuleFor(x => x.SubmittedAt)
                .GreaterThanOrEqualTo(x => x.StartedAt)
                .When(x => x.SubmittedAt.HasValue)
                .WithMessage("SubmittedAt must be after StartedAt");
        }
    }
}