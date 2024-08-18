namespace Common.Base.Interfaces.Entities;

public interface IMongoEntity<T> : IEntity<T> 
{
    void SetNewUniqueIdValue();
}