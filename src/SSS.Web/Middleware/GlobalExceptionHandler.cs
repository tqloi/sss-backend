using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Common.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace SSS.Middleware
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
     : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken ct)
        {
            var (status, type, title, message) = ex switch
            {
                // nghiệp vụ
                NotFoundException => (404, "app/not-found", "Not Found", ex.Message),
                ForbiddenException => (403, "app/forbidden", "Forbidden", ex.Message),
                UnauthorizedException => (401, "auth/unauthorized", "Unauthorized", ex.Message),
                UnauthorizedAccessException => (401, "auth/unauthorized-access", "Unauthorized Access", ex.Message),
                //InvalidOperationException => (400, "app/invalid-operation", "Invalid Operation", ex.Message),
                FormatException => (400, "app/invalid-format", "Invalid Format", ex.Message),

                // dữ liệu
                ConcurrencyException => (409, "app/concurrency", "Concurrency conflict", ex.Message),
                DbUpdateConcurrencyException
                                            => (409, "db/concurrency", "Concurrency conflict", "Data changed. Try again."),
                DbUpdateException => (500, "server/db-error", "Database error", "Database error occurred."),

                ValidationException =>
                    (400, "app/validation-error", "Validation Error", ex.Message),

                ConflictException =>
                    (409, "app/conflict", "Conflict", ex.Message),

                // mặc định
                //_ => (500, "server/unexpected", "Internal Server Error", "An unexpected error occurred.")
            };

            logger.LogError(ex, "Error at {Path} → {Status}", ctx.Request.Path, status);

            var problem = new
            {
                type,
                title,
                status,
                message,
                traceId = ctx.TraceIdentifier
            };

            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/problem+json";
            await ctx.Response.WriteAsJsonAsync(problem, cancellationToken: ct);
            return true;
        }
    }
}
