using MediatR;

namespace SSS.Application.Features.UserManagement.GetAllUsers
{
    public sealed record GetAllUsersQuery(
        int PageIndex = 1,
        int PageSize = 10,
        string? Name = null,
        string? Role = null
    ) : IRequest<GetAllUsersResponse>;
}
