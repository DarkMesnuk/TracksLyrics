namespace Common.Base.Interfaces.Models;

public interface IEntityModelDateTime
{
    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }
}