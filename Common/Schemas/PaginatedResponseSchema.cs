namespace Common.Schemas;

public class PaginatedResponseSchema<T> : IPaginatedResponseSchema<T>
{
    public int Count { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<T> Dtos { get; set; }
}

public interface IPaginatedResponseSchema<T>
{
    public int Count { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<T> Dtos { get; set; }
}