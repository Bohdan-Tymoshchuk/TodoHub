using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoHub.Domain.Abstractions;
using TodoHub.Domain.Abstractions.Repositories;
using TodoHub.Infrastructure.Persistence;
using TodoHub.Infrastructure.Persistence.Interceptors;
using TodoHub.Infrastructure.Persistence.Repositories;

namespace TodoHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<TodoDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoCollectionRepository, TodoCollectionRepository>();
        services.AddScoped<ITodoSharedCollectionRepository, TodoSharedCollectionRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }
}