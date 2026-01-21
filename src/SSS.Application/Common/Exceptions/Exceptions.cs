namespace LecX.Application.Common.Execption
{
    public sealed class NotFoundException(string message) : Exception(message);
    public sealed class ForbiddenException(string message) : Exception(message);
    public sealed class ConcurrencyException(string message) : Exception(message);
    public sealed class UnauthorizedException(string message) : Exception(message);
}
