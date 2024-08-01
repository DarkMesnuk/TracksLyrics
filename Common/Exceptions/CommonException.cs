using Common.Models;
using Common.Helpers;

namespace Common.Exceptions;

public abstract class CommonException : Exception
{
    protected CommonException() : this(StatusCodes.SomethingWentWrong)
    {
    }
    
    protected CommonException(string error) : this(StatusCodes.SomethingWentWrong, error)
    {
    }
    
    protected CommonException(StatusCodes code, string error) : this(code)
    {
        ErrorResponse.SetAdditionalMessage(error);
    }
    
    protected CommonException(StatusCodes code)
    {
        var result = ApplicationResponseStatuses.Statuses.GetValueOrDefault(code);
        ErrorResponse = new ApplicationResponse().SetData(result);
    }

    public ApplicationResponse ErrorResponse { get; }
}