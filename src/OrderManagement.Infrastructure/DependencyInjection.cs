using Microsoft.Extensions.DependencyInjection;

namespace OrderManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register infrastructure services here
        // Example: services.AddScoped<IRepository, Repository>();
        
        return services;
    }
}
