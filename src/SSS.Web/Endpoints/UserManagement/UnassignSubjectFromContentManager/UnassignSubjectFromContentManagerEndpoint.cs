using FastEndpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserManagement.UnassignSubjectFromContentManager;

namespace SSS.Web.Endpoints.UserManagement.UnassignSubjectFromContentManager
{
    public sealed class UnassignSubjectFromContentManagerEndpoint(
        ISender sender,
        IAppDbContext dbContext) : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Delete("/api/admin/users/{id}/subject");
            Roles("Admin");
            Description(d => d.WithTags("UserManagement"));
            Summary(s =>
            {
                s.Summary = "Unassign subject from content manager";
                s.Description = "Removes active subject assignment(s) from a content manager user. Provide subjectId query to remove one subject only.";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var contentManagerId = Route<string>("id");
            var subjectId = Query<long?>("subjectId", false);

            if (string.IsNullOrWhiteSpace(contentManagerId))
            {
                await SendAsync(new { message = "Invalid content manager id" }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            var userExists = await dbContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == contentManagerId, ct);

            if (!userExists)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var roleNames = await (
                from ur in dbContext.UserRoles.AsNoTracking()
                join r in dbContext.Roles.AsNoTracking() on ur.RoleId equals r.Id
                where ur.UserId == contentManagerId
                select r.Name
            ).ToListAsync(ct);

            var isContentManager = roleNames.Any(roleName =>
                !string.IsNullOrWhiteSpace(roleName) &&
                roleName.Replace(" ", "").Equals("ContentManager", StringComparison.OrdinalIgnoreCase));

            if (!isContentManager)
            {
                await SendAsync(new { message = "Target user is not a content manager." }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            var unassigned = await sender.Send(new UnassignSubjectFromContentManagerCommand
            {
                ContentManagerId = contentManagerId,
                SubjectId = subjectId,
            }, ct);

            if (!unassigned)
            {
                await SendAsync(new { message = "Failed to unassign subject." }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            await SendNoContentAsync(ct);
        }
    }
}
