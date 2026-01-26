using AutoMapper;
using SSS.Application.Features.UserProfile.ChangePassword;
using SSS.Application.Features.UserProfile.Common;
using SSS.Application.Features.UserProfile.UpdateProfile;
using SSS.Domain.Enums;
using SSS.WebApi.Endpoints.Users.ChangePassword;
using SSS.WebApi.Endpoints.Users.GetProfile;
using SSS.WebApi.Endpoints.Users.UpdateProfile;

namespace SSS.WebApi.Endpoints.Users.Common;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // GetProfile
        CreateMap<UserProfileDto, GetProfileResponse>();

        // UpdateProfile
        CreateMap<UpdateProfileRequest, UpdateProfileCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => ParseGender(src.Gender)));
        CreateMap<UserProfileDto, UpdateProfileResponse>();

        // ChangePassword
        CreateMap<ChangePasswordRequest, ChangePasswordCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        CreateMap<ChangePasswordResult, ChangePasswordResponse>();
    }

    private static Gender? ParseGender(string? gender)
    {
        if (string.IsNullOrEmpty(gender))
            return null;

        return Enum.TryParse<Gender>(gender, true, out var parsedGender)
            ? parsedGender
            : null;
    }
}
