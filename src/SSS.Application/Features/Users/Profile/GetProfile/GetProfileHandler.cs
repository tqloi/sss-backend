using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.Users.Profile.GetProfile;

public class GetProfileHandler : IRequestHandler<GetProfileQuery, ProfileResult>
{
    private readonly UserManager<User> _userManager;

    public GetProfileHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ProfileResult> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            throw new KeyNotFoundException("User not found");

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
