using System.Net;
using Common.Helpers;

namespace Common.Exceptions;

[HttpStatusCode(HttpStatusCode.BadRequest)]
public class UpdateFailedException() : CommonException(StatusCodes.UpdateFailed);