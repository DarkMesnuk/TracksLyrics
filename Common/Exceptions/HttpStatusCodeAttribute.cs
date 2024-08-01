using System.Net;

namespace Common.Exceptions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class HttpStatusCodeAttribute(
    HttpStatusCode statusCode
    ) : Attribute
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}