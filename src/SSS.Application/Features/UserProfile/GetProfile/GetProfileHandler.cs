using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Application.Features.UserProfile.Common;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.UserProfile.GetProfile;

public sealed class GetProfileHandler(
    UserManager<User> userManager
) : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new InvalidOperationException("User not found");

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
