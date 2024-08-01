using System.ComponentModel.DataAnnotations;
using Common.Base.Interfaces.Requests;

namespace Common.Base.Requests;

public abstract class BasePaginatedRequest : IPaginatedRequest
{
    public int PageNumber { get; init; } = 0;
    public int PageSize { get; init; } = 10;
    [RegularExpression(@"^[a-zA-Z0-9'\sа-яА-ЯіІ]*$", ErrorMessage = "Invalid characters in the search field")]
    public string? Search { get; init; }
}