namespace Common.Base.Interfaces.Models;

public interface IEntityModel<T> : IEntityModel
{
    T Id { get; set; }
}

public interface IEntityModel
{
}