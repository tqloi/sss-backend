using FastEndpoints;
using FluentValidation;
using System.Text.Json;

namespace SSS.WebApi.Endpoints.UserSubjectStats.Create;

public class CreateUserSubjectStatValidator : Validator<CreateUserSubjectStatRequest>
{
    public CreateUserSubjectStatValidator()
    {
        RuleFor(x => x.SubjectId)
            .GreaterThan(0).WithMessage("SubjectId must be greater than 0.");

        RuleFor(x => x.ProficiencyLevel)
            .InclusiveBetween(0, 100).WithMessage("ProficiencyLevel must be between 0 and 100.")
            .When(x => x.ProficiencyLevel.HasValue);

        RuleFor(x => x.TotalHoursSpent)
            .GreaterThanOrEqualTo(0).WithMessage("TotalHoursSpent must be non-negative.")
            .When(x => x.TotalHoursSpent.HasValue);

        RuleFor(x => x.WeakNodeIds)
            .Must(BeValidJsonOrNull).WithMessage("WeakNodeIds must be valid JSON (e.g., [1,2,3]).")
            .When(x => !string.IsNullOrWhiteSpace(x.WeakNodeIds));
    }

    private static bool BeValidJsonOrNull(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return true;
        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
