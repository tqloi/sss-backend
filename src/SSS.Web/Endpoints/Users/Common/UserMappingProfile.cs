using SSS.Application.Features.Users.Profile;
using SSS.Application.Features.Users.Profile.ChangePassword;
using SSS.Application.Features.Users.Profile.UpdateProfile;
using SSS.Application.Features.Users.Profile.UploadAvatar;
using SSS.WebApi.Endpoints.Users.Profile.ChangePassword;
using SSS.WebApi.Endpoints.Users.Profile.GetProfile;
using SSS.WebApi.Endpoints.Users.Profile.UpdateProfile;
using SSS.WebApi.Endpoints.Users.Profile.UploadAvatar;

namespace SSS.WebApi.Endpoints.Users.Common;

public class UserMappingProfile : AutoMapper.Profile
{
    public UserMappingProfile()
    {
        // Get Profile
        CreateMap<ProfileResult, GetProfileResponse>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.HasValue ? src.Gender.ToString() : null));

        // Update Profile
        CreateMap<UpdateProfileRequest, UpdateProfileCommand>();
        CreateMap<ProfileResult, UpdateProfileResponse>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.HasValue ? src.Gender.ToString() : null));

        // Change Password
        CreateMap<ChangePasswordResult, ChangePasswordResponse>();

        // Upload Avatar
        CreateMap<UploadAvatarResult, UploadAvatarResponse>();
    }
}
