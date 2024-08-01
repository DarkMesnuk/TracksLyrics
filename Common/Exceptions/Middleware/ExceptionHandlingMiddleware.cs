﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Reflection;
using System.Text.Json;
using Common.Models;
using StatusCodes=Common.Helpers.StatusCodes;

namespace Common.Exceptions.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next, 
    ILogger<ExceptionHandlingMiddleware> logger
    )
{
    public virtual async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (CommonException ex)
        {
            logger.LogWarning(ex.GetBaseException(), "Request \"{RequestUri}\" has failed", context.Request.GetDisplayUrl());

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)GetStatusCode(ex);
                
            await context.Response.WriteAsync(SerializeResponse(ex.ErrorResponse));
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex.GetBaseException(), "Request \"{RequestUri}\" validation has failed", context.Request.GetDisplayUrl());

            var response = new ApplicationResponse().SetData(StatusCodes.ValidationFailed);
            
            foreach (var error in ex.Errors)
                response.SetAdditionalMessage($"Field: {error.PropertyName}, Error: {error.ErrorMessage}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsync(SerializeResponse(response));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex.GetBaseException(), "Request \"{RequestUri}\" has failed with internal error", context.Request.GetDisplayUrl());

            var response = new ApplicationResponse().SetData(StatusCodes.SomethingWentWrong, ex.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(SerializeResponse(response));
        }
    }

    private HttpStatusCode GetStatusCode(CommonException ex)
    {
        var statusCodeAttribute = ex.GetType().GetCustomAttribute<HttpStatusCodeAttribute>();
        return statusCodeAttribute?.StatusCode ?? HttpStatusCode.BadRequest;
    }

    private string SerializeResponse(ApplicationResponse response)
    {
        return JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}