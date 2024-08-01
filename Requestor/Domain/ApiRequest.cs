namespace Requestor.Domain;

public class ApiRequest
{
    public required HttpMethod Type { get; init; }
    public required string Url { get; init; }
    public object? Data { get; init; }
    public string? AccessToken { get; init; }
}