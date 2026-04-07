using AutoMapper;
using SSS.Application.Features.UserGamifications.Common;
using SSS.Application.Features.UserGamifications.CreateUserGamification;
using SSS.Application.Features.UserGamifications.UpdateUserGamification;
using SSS.WebApi.Endpoints.UserGamifications.Create;
using SSS.WebApi.Endpoints.UserGamifications.Get;
using SSS.WebApi.Endpoints.UserGamifications.GetAll;
using SSS.WebApi.Endpoints.UserGamifications.Update;

namespace SSS.WebApi.Endpoints.UserGamifications.Common;

public class UserGamificationMappingProfile : Profile
{
    public UserGamificationMappingProfile()
    {
        // Get
        CreateMap<UserGamificationDto, GetUserGamificationResponse>();

        // GetAll (Admin)
        CreateMap<UserGamificationDto, GetAllUserGamificationsResponse.UserGamificationItem>();

        // Create
        CreateMap<CreateUserGamificationRequest, CreateUserGamificationCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        CreateMap<UserGamificationDto, CreateUserGamificationResponse>();

        // Update
        CreateMap<UpdateUserGamificationRequest, UpdateUserGamificationCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        CreateMap<UserGamificationDto, UpdateUserGamificationResponse>();
    }
}
