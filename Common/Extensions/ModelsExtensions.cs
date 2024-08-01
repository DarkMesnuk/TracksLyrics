using Common.Base.Interfaces.Models;

namespace Common.Extensions;

public static class ModelsExtensions
{
    public static bool IsNullOrDefault(this IEntityModel? model)
    {
        return model == null;
    }
    
    public static bool IsNullOrDefault<T>(this IEntityModel<T>? model)
    {
        if (model == null) 
            return true;

        if ((typeof(T) == typeof(int) || typeof(T) == typeof(long)) && model.Id!.Equals(0))
            return true;
        
        return false;
    }

    public static bool IsAvailable<T>(this IEntityModel<T> model)
    {
        return !model.IsNullOrDefault();
    }
}