using System.Net;
using Common.Helpers;

namespace Common.Exceptions;

[HttpStatusCode(HttpStatusCode.BadRequest)]
public class CreationFailedException(string entityType = "") : CommonException(StatusCodes.CreationFailed, entityType);