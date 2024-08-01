namespace Common.Base.Interfaces.Entities;

public interface IEntity<T> : IEntity
{
    T Id { get; set; }
}

public interface IEntity
{
}