using FluentValidation;
using TodoHub.API.Models;
using TodoHub.Application.DTOs;
using TodoHub.Application.Services;
using TodoHub.Application.Services.Abstractions;
using TodoHub.Domain.Pagination;

namespace TodoHub.API.Endpoints;

public static class UserEndpoints
{
     public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/users",
            async (UserDto request, IUserService userService, IValidator<UserDto> validator, CancellationToken cancellationToken) =>
            {
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return Results.BadRequest(ApiResponse<UserDto>.ErrorResult("Validation failed", errors));
                }

                var result = await userService.CreateAsync(request, cancellationToken);
                return Results.Created($"/api/users/{result.Id}", ApiResponse<UserDto>.SuccessResult(result));
            })
            .WithTags("Users")
            .WithName("CreateUser")
            .Produces<UserDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create user")
            .WithDescription("Creates a new user in the system");
        
        app.MapGet("api/users",
            async (IUserService userService, CancellationToken cancellationToken) =>
            {
                var result = await userService.GetAllAsync(cancellationToken);
                return Results.Ok(ApiResponse<List<UserDto>>.SuccessResult(result));
            })
            .WithTags("Users")
            .WithName("GetUsers")
            .Produces<List<UserDto>>(StatusCodes.Status200OK)
            .WithSummary("Get all users")
            .WithDescription("Retrieves a paginated list of all users in the system");
    }
}