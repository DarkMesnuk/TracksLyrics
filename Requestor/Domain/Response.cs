namespace Requestor.Domain;

public class Response
{
    public bool IsSuccess { get; set; } = true;

    public string DisplayMessage { get; set; } = string.Empty;

    public List<string> ErrorMessages { get; set; } = new();
    public int HttpCode { get; set; }
}

public class Response<T> : Response
{
    public T Result { get; set; }
}