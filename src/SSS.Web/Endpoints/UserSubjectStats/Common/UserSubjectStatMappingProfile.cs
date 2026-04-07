using AutoMapper;
using SSS.Application.Features.UserSubjectStats.Common;
using SSS.Application.Features.UserSubjectStats.CreateUserSubjectStat;
using SSS.Application.Features.UserSubjectStats.UpdateUserSubjectStat;
using SSS.WebApi.Endpoints.UserSubjectStats.Create;
using SSS.WebApi.Endpoints.UserSubjectStats.GetById;
using SSS.WebApi.Endpoints.UserSubjectStats.Update;

namespace SSS.WebApi.Endpoints.UserSubjectStats.Common;

public class UserSubjectStatMappingProfile : Profile
{
    public UserSubjectStatMappingProfile()
    {
        // GetById
        CreateMap<UserSubjectStatDto, GetUserSubjectStatByIdResponse>();

        // Create
        CreateMap<CreateUserSubjectStatRequest, CreateUserSubjectStatCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        CreateMap<UserSubjectStatDto, CreateUserSubjectStatResponse>();

        // Update
        CreateMap<UpdateUserSubjectStatRequest, UpdateUserSubjectStatCommand>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        CreateMap<UserSubjectStatDto, UpdateUserSubjectStatResponse>();
    }
}
