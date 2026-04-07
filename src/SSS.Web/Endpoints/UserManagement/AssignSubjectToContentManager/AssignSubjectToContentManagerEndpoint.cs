using FastEndpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserManagement.AssignSubjectToContentManager;
using System.Security.Claims;

namespace SSS.Web.Endpoints.UserManagement.AssignSubjectToContentManager
{
    public sealed class AssignSubjectToContentManagerEndpoint(
        ISender sender,
        IAppDbContext dbContext) : Endpoint<AssignSubjectToContentManagerCommand>
    {
        public override void Configure()
        {
            Put("/api/admin/users/{id}/subject");
            Roles("Admin");
            Description(d => d.WithTags("UserManagement"));
            Summary(s =>
            {
                s.Summary = "Assign a subject to content manager";
                s.Description = "Assigns exactly one active subject to a content manager user.";
            });
        }

        public override async Task HandleAsync(AssignSubjectToContentManagerCommand req, CancellationToken ct)
        {
            req.ContentManagerId = Route<string>("id") ?? req.ContentManagerId;
            req.AssignedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(req.ContentManagerId))
            {
                await SendAsync(new { message = "Invalid content manager id" }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            if (req.SubjectId <= 0)
            {
                await SendAsync(new { message = "SubjectId must be greater than 0" }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            var userExists = await dbContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == req.ContentManagerId, ct);

            if (!userExists)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            var subjectExists = await dbContext.LearningSubjects
                .AsNoTracking()
                .AnyAsync(x => x.Id == req.SubjectId && x.IsActive, ct);

            if (!subjectExists)
            {
                await SendAsync(new { message = "Subject not found or inactive" }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            var roleNames = await (
                from ur in dbContext.UserRoles.AsNoTracking()
                join r in dbContext.Roles.AsNoTracking() on ur.RoleId equals r.Id
                where ur.UserId == req.ContentManagerId
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

            var assigned = await sender.Send(req, ct);

            if (!assigned)
            {
                await SendAsync(new { message = "Failed to assign subject." }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            await SendNoContentAsync(ct);
        }
    }
}
