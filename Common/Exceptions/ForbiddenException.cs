using System.Net;
using Common.Helpers;

namespace Common.Exceptions;

[HttpStatusCode(HttpStatusCode.Forbidden)]
public class ForbiddenException() : CommonException(StatusCodes.AccessDenied)
{
    public override string Message => "You don't have permission to access this request'";
}