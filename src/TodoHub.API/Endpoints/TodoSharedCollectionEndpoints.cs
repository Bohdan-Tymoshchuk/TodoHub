using FluentValidation;
using TodoHub.API.Models;
using TodoHub.Application.DTOs;
using TodoHub.Application.Services.Abstractions;

namespace TodoHub.API.Endpoints;

public static class TodoSharedCollectionEndpoints
{
    public static void MapTodoSharedCollectionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/todo-share-collections/{todoCollectionId}", async (Guid todoCollectionId, ITodoSharedCollectionService service, CancellationToken cancellationToken) =>
        {
            var result = await service.GetTodoSharedCollectionAsync(todoCollectionId, cancellationToken);

            return Results.Ok(ApiResponse<List<TodoSharedCollectionDto>>.SuccessResult(result));
        })
        .WithTags("TodoShareCollection")
        .WithName("GetTodoSharedCollectionById")
        .Produces<ApiResponse<List<TodoSharedCollectionDto>>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithDescription("Get a list of users who have access to a specific Todo collection by its ID");

        app.MapPost("/api/todo-share-collections", async (TodoSharedCollectionDto request, ITodoSharedCollectionService service, IValidator<TodoSharedCollectionDto> validator, CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!(validationResult.IsValid))
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Results.BadRequest(ApiResponse<TodoCollectionDto>.ErrorResult("Validation failed", errors));
            }
            
            var result = await service.ShareTodoCollectionAsync(request, cancellationToken);
            
            return Results.Created($"/api/todo-share-collections/{result.Id}", ApiResponse<TodoSharedCollectionDto>.SuccessResult(result));
        })
        .WithTags("TodoShareCollection")
        .WithName("CreateTodoShareCollection")
        .Produces<ApiResponse<TodoSharedCollectionDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithDescription("Share a Todo collection with another user by providing the collection ID and the user ID");

        app.MapDelete("/api/todo-share-collections/{id}", async (Guid id, ITodoSharedCollectionService service, CancellationToken cancellationToken) =>
        {
            await service.RevokeTodoCollectionAccessAsync(id, cancellationToken);
            
            return Results.NoContent();
        })
        .WithTags("TodoShareCollection")
        .WithName("DeleteTodoShareCollection")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .WithDescription("Revoke access to a Todo collection for a specific user by providing the share ID");
    }
}