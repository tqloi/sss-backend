using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Application.Features.UserProfile.Common;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.UserProfile.UpdateProfile;

public sealed class UpdateProfileHandler(
    UserManager<User> userManager
) : IRequestHandler<UpdateProfileCommand, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new InvalidOperationException("User not found");

        // Update fields if provided
        if (request.FirstName is not null)
            user.FirstName = request.FirstName;

        if (request.LastName is not null)
            user.LastName = request.LastName;

        if (request.AvatarUrl is not null)
            user.AvatarUrl = request.AvatarUrl;

        if (request.Address is not null)
            user.Address = request.Address;

        if (request.Dob.HasValue)
            user.Dob = request.Dob;

        if (request.Gender.HasValue)
            user.Gender = request.Gender;

        if (request.PhoneNumber is not null)
            user.PhoneNumber = request.PhoneNumber;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(result.Errors.First().Description);

        return new UserProfileDto
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarUrl = user.AvatarUrl,
            Address = user.Address,
            Dob = user.Dob,
            Gender = user.Gender?.ToString(),
            PhoneNumber = user.PhoneNumber
        };
    }
}
