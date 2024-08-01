namespace Common.Base.Interfaces.Requests;

public interface IPaginatedRequest
{
    int PageNumber { get; init; }
    int PageSize { get; init; }
    string? Search { get; init; }
}