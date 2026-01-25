using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.UserSubjectStats.Update;

public class UpdateUserSubjectStatValidator : Validator<UpdateUserSubjectStatRequest>
{
    public UpdateUserSubjectStatValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.ProficiencyLevel)
            .InclusiveBetween(0, 100).WithMessage("ProficiencyLevel must be between 0 and 100.")
            .When(x => x.ProficiencyLevel.HasValue);

        RuleFor(x => x.TotalHoursSpent)
            .GreaterThanOrEqualTo(0).WithMessage("TotalHoursSpent must be non-negative.")
            .When(x => x.TotalHoursSpent.HasValue);
    }
}
