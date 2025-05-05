using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DiConfig
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<CustomerService>();
        services.AddScoped<OrderService>();

        return services;
    }
}