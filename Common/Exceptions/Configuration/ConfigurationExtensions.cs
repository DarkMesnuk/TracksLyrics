using Microsoft.AspNetCore.Builder;
using Common.Exceptions.Middleware;

namespace Common.Exceptions.Configuration;

public static class ConfigurationExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}