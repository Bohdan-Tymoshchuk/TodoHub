using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoHub.API.Models;
using TodoHub.Application.DTOs;
using TodoHub.Application.Services;
using TodoHub.Application.Services.Abstractions;
using TodoHub.Domain.Pagination;

namespace TodoHub.API.Endpoints;

public static class TodoCollectionEndpoints
{
    public static void MapTodoCollectionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/todo-collections",
                async (TodoCollectionDto request, ITodoCollectionService todoCollectionService, IValidator<TodoCollectionDto> validator, CancellationToken cancellationToken) =>
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!(validationResult.IsValid))
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Results.BadRequest(ApiResponse<TodoCollectionDto>.ErrorResult("Validation failed", errors));
                }

                var result = await todoCollectionService.CreateAsync(request, cancellationToken);

                return Results.Created($"/api/todoCollection/{result.Id}", ApiResponse<TodoCollectionDto>.SuccessResult(result));
            })
            .WithTags("TodoCollections")
            .WithName("CreateTodoCollection")
            .Produces<TodoCollectionDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create todo collection")
            .WithDescription("Creates a new todo collection");
        
        app.MapPut("api/todo-collections/{id:guid}",
            async (Guid id, TodoCollectionDto request, ITodoCollectionService todoCollectionService, IValidator<TodoCollectionDto> validator, CancellationToken cancellationToken) =>
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!(validationResult.IsValid))
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Results.BadRequest(ApiResponse<TodoCollectionDto>.ErrorResult("Validation failed", errors));
                }

                var result = await todoCollectionService.UpdateAsync(id, request, cancellationToken);

                return Results.Ok(ApiResponse<TodoCollectionDto>.SuccessResult(result));
            })
            .WithTags("TodoCollections")
            .WithName("UpdateTodoCollection")
            .Produces<TodoCollectionDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update todo collection")
            .WithDescription("Updates an existing todo collection by ID");
        
        app.MapDelete("api/todo-collections/{id:guid}",
            async (Guid id, ITodoCollectionService todoCollectionService, CancellationToken cancellationToken) =>
            {
                await todoCollectionService.DeleteAsync(id, cancellationToken);
                
                return Results.NoContent();
            })
            .WithTags("TodoCollections")
            .WithName("DeleteTodoCollection")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete todo collection")
            .WithDescription("Deletes a todo collection by ID");

        app.MapGet("api/todo-collections/{id:guid}",
            async (Guid id, ITodoCollectionService todoCollectionService, CancellationToken cancellationToken) =>
            {
                var result = await todoCollectionService.GetByIdAsync(id, cancellationToken);
                if (result is null)
                    return Results.NotFound();

                return Results.Ok(ApiResponse<TodoCollectionDto>.SuccessResult(result));
            })
            .WithTags("TodoCollections")
            .WithName("GetTodoCollectionById")
            .Produces<TodoCollectionDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get todo collection by ID")
            .WithDescription("Retrieves a todo collection by its ID for the authenticated user");
        
        app.MapGet("api/todo-collections",
            async ([AsParameters] PaginationRequest request, ITodoCollectionService todoCollectionService, CancellationToken cancellationToken) =>
            {
                var result = await todoCollectionService.GetAllAsync(request, cancellationToken);
                
                return Results.Ok(ApiResponse<PaginatedResult<TodoCollectionDto>>.SuccessResult(result));
            })
            .WithTags("TodoCollections")
            .WithName("GetUserTodoCollections")
            .Produces<PaginatedResult<TodoCollectionDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get user todo collections")
            .WithDescription("Retrieves a paginated list of todo collections for the authenticated user");
    }
}