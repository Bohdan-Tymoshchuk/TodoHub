using Microsoft.Extensions.DependencyInjection;
using TodoHub.Application.Services;
using TodoHub.Application.Services.Abstractions;

namespace TodoHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITodoCollectionService, TodoCollectionService>();
        services.AddScoped<ITodoSharedCollectionService, TodoSharedCollectionService>();
        
        return services;
    }
}