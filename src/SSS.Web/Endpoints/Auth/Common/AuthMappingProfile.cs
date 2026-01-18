using AutoMapper;
using SSS.Application.Features.Auth.Common;
using SSS.Application.Features.Auth.Login;
using SSS.Application.Features.Auth.Register;
using SSS.Application.Features.Auth.ResetPassword;
using SSS.WebApi.Endpoints.Auth.Login;
using SSS.WebApi.Endpoints.Auth.Refresh;
using SSS.WebApi.Endpoints.Auth.Register;
using SSS.WebApi.Endpoints.Auth.ResetPassword;

namespace SSS.WebApi.Endpoints.Auth.Common
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            //Login
            CreateMap<LoginRequest, LoginCommand>();
            CreateMap<AuthResult, LoginResponse>();

            //Refresh token
            CreateMap<AuthResult, RefreshResponse>();

            //Register 
            CreateMap<RegisterRequest, RegisterCommand>();
            CreateMap<RegisterResult, RegisterResponse>();

            //Register 
            CreateMap<RegisterRequest, RegisterCommand>();
            CreateMap<RegisterResult, RegisterResponse>();

            //Password reset
            CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
        }
    }
}
