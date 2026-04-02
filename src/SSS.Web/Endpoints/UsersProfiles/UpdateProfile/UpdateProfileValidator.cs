using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Users.UpdateProfile;

public class UpdateProfileValidator : Validator<UpdateProfileRequest>
{
    private static readonly string[] ValidGenders = ["Male", "Female", "Other"];

    public UpdateProfileValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.")
            .When(x => x.FirstName is not null);

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.")
            .When(x => x.LastName is not null);

        RuleFor(x => x.AvatarUrl)
            .Must(BeAValidUrl).WithMessage("Avatar URL must be a valid URL.")
            .When(x => !string.IsNullOrEmpty(x.AvatarUrl));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.")
            .When(x => x.Address is not null);

        RuleFor(x => x.Dob)
            .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.")
            .GreaterThan(DateTime.UtcNow.AddYears(-120)).WithMessage("Date of birth is not valid.")
            .When(x => x.Dob.HasValue);

        RuleFor(x => x.Gender)
            .Must(g => ValidGenders.Contains(g, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Gender must be 'Male', 'Female', or 'Other'.")
            .When(x => !string.IsNullOrEmpty(x.Gender));

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9]{9,15}$").WithMessage("Phone number is not valid.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }

    private static bool BeAValidUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
