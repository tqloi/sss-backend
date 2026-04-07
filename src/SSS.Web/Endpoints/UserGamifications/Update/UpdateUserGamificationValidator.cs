using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.UserGamifications.Update;

public class UpdateUserGamificationValidator : Validator<UpdateUserGamificationRequest>
{
    public UpdateUserGamificationValidator()
    {
        RuleFor(x => x.CurrentStreak)
            .GreaterThanOrEqualTo(0).WithMessage("CurrentStreak must be non-negative.")
            .When(x => x.CurrentStreak.HasValue);

        RuleFor(x => x.LongestStreak)
            .GreaterThanOrEqualTo(0).WithMessage("LongestStreak must be non-negative.")
            .When(x => x.LongestStreak.HasValue);

        RuleFor(x => x.TotalExp)
            .GreaterThanOrEqualTo(0).WithMessage("TotalExp must be non-negative.")
            .When(x => x.TotalExp.HasValue);

        RuleFor(x => x.LastActiveDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).WithMessage("LastActiveDate cannot be in the future.")
            .When(x => x.LastActiveDate.HasValue);
    }
}
