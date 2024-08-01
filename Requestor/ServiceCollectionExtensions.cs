using Microsoft.Extensions.DependencyInjection;

namespace Requestor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestor(this IServiceCollection services)
    {
        services.AddHttpClient();
        
        return services
            .AddScoped<ISender, Sender>();
    }
}
