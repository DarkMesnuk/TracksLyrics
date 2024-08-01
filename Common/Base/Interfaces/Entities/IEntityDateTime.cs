namespace Common.Base.Interfaces.Entities;

public interface IEntityDateTime
{
    DateTime CreatedAt { get; set; }
    DateTime ModifiedAt { get; set; }   
}