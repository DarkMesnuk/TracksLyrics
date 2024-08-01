using Common.Base.Interfaces.Entities;

namespace Common.Extensions;

public static class EntitiesExtensions
{
    public static bool IsNullOrDefault<T>(this IEntity<T>? model)
    {
        if (model == null) 
            return true;

        if ((typeof(T) == typeof(int) || typeof(T) == typeof(long)) && model.Id.Equals(0))
            return true;
        
        return false;
    }

    public static bool IsAvailable<T>(this IEntity<T>? model)
    {
        return !model.IsNullOrDefault();
    }
}