using System.Net;
using Common.Helpers;

namespace Common.Exceptions;

[HttpStatusCode(HttpStatusCode.BadRequest)]
public class SomethingWentWrongException(string message = "Something went wrong") : CommonException(StatusCodes.SomethingWentWrong)
{
    public override string Message => message;
}