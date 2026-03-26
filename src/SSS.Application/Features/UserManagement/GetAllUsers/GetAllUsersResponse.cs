using SSS.Application.Common.Dtos;
using SSS.Application.Features.UserManagement.Common;

namespace SSS.Application.Features.UserManagement.GetAllUsers
{
    public sealed class GetAllUsersResponse
    {
        public PaginatedResponse<UserDto> Users { get; set; } =
            new(1, 10, 0, new List<UserDto>());
    }
}
