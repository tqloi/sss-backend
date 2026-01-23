using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.Users.Profile.UpdateProfile;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, ProfileResult>
{
    private readonly UserManager<User> _userManager;

    public UpdateProfileHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ProfileResult> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new KeyNotFoundException("User not found");

        // Update fields if provided
        if (request.FirstName is not null)
            user.FirstName = request.FirstName;
        if (request.LastName is not null)
            user.LastName = request.LastName;
        if (request.Address is not null)
            user.Address = request.Address;
        if (request.Dob.HasValue)
            user.Dob = request.Dob;
        if (request.Gender.HasValue)
            user.Gender = request.Gender;
        if (request.PhoneNumber is not null)
            user.PhoneNumber = request.PhoneNumber;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return new ProfileResult
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarUrl = user.AvatarUrl,
            Address = user.Address,
            Dob = user.Dob,
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber
        };
    }
}
