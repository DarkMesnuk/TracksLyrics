using Common.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Common.Models;

public class ApplicationResponse
{
    public int HttpCode { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public List<string> Errors { get; } = new List<string>();

    public bool IsSucceeded => HttpCode == 200;

    public ApplicationResponse()
    {
        HttpCode = 200;
    }

    public ApplicationResponse(string status, string description, int httpCode = 200)
    {
        Status = status;
        Description = description;
        HttpCode = httpCode;
    }

    public void SetData(string status, string message, int httpCode = 200)
    {
        Status = status;
        Description = message;
        HttpCode = httpCode;
    }

    public ApplicationResponse SetAdditionalMessage(string message)
    {
        Errors.Add(message);

        return this;
    }

    public ApplicationResponse SetAdditionalMessage(StatusCodes code)
    {
        var result = ApplicationResponseStatuses.Statuses.GetValueOrDefault(code);
        SetAdditionalMessage(result.Description);

        return this;
    }

    public ApplicationResponse SetData(ApplicationResponse response)
    {
        Status = response.Status;
        Description = response.Description;
        HttpCode = response.HttpCode;
        Type = response.Type;

        return this;
    }
    
    public ApplicationResponse SetData(StatusCodes code)
    {
        var result = ApplicationResponseStatuses.Statuses.GetValueOrDefault(code);
        SetData(result);

        return this;
    }

    public ApplicationResponse SetData(StatusCodes code, string message)
    {
        SetData(code);
        
        if (!string.IsNullOrEmpty(message))
        {
            Description = message;
        }

        return this;
    }
    
    public virtual IActionResult GetActionResult()
    {
        var result = new ObjectResult(GenerateDefaultObjectForActionResult())
        {
            StatusCode = HttpCode
        };
        
        return result;
    }

    protected object GenerateDefaultObjectForActionResult() => new { 
        Status,
        Description,
        Errors,
        Type
    };
}

public class ApplicationResponse<TResponse> : ApplicationResponse
    where TResponse : ApplicationResponse
{
    protected ApplicationResponse()
    {
        SetData(StatusCodes.Success);
    }
    
    public new TResponse SetAdditionalMessage(string message)
        => (TResponse) base.SetAdditionalMessage(message);
    
    public new TResponse SetAdditionalMessage(StatusCodes code)
        => (TResponse) base.SetAdditionalMessage(code);
    
    public new TResponse SetData(ApplicationResponse response)
        => (TResponse) base.SetData(response);
    
    public new TResponse SetData(StatusCodes code)
        => (TResponse) base.SetData(code);

    public new TResponse SetData(StatusCodes code, string message)
        => (TResponse) base.SetData(code, message);
}
