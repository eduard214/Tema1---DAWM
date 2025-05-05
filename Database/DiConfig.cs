using Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class DiConfig
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>();
        services.AddScoped<DbContext, DatabaseContext>();
        
        return services;
    }
}