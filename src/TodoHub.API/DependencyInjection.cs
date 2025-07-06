using FluentValidation;
using Microsoft.OpenApi.Models;
using TodoHub.API.Contexts;
using TodoHub.API.Middlewares;
using TodoHub.API.OperationTransformers;
using TodoHub.API.Validators;
using TodoHub.Application.DTOs;
using TodoHub.Domain.Contexts;

namespace TodoHub.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
        
        services.AddOpenApi(options =>
        {
            options.AddOperationTransformer<AddRequiredHeaderParameter>();
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "TodoHub API",
                    Version = "v1",
                    Description = "API for managing todo lists and tasks"
                };
                return Task.CompletedTask;
            });
        });
        
        services.AddScoped<IValidator<UserDto>, UserValidator>();
        services.AddScoped<IValidator<TodoCollectionDto>, TodoCollectionValidator>();
        services.AddScoped<IValidator<TodoTaskDto>, TodoTaskValidator>();
        services.AddScoped<IValidator<TodoSharedCollectionDto>, TodoSharedCollectionValidator>();
        
        return services;
    }
    
    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.UseExceptionHandler(_ => { });
        app.MapOpenApi("/openapi/{documentName}/openapi.json");
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1/openapi.json", "TodoHub API");
        });
        
        return app;
    }
}