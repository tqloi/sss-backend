using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Common.Exceptions
{
    public sealed class NotFoundException(string message) : Exception(message);
    public sealed class ForbiddenException(string message) : Exception(message);
    public sealed class ConcurrencyException(string message) : Exception(message);
    public sealed class UnauthorizedException(string message) : Exception(message);
}
