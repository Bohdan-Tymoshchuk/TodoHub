using BuildingBlocks.Exceptions;
using TodoHub.Application.DTOs;
using TodoHub.Application.Extensions;
using TodoHub.Application.Services.Abstractions;
using TodoHub.Domain;
using TodoHub.Domain.Abstractions;
using TodoHub.Domain.Abstractions.Repositories;
using TodoHub.Domain.Contexts;
using TodoHub.Domain.Entities;
using TodoHub.Domain.Exceptions;

namespace TodoHub.Application.Services;

public class TodoSharedCollectionService(ITodoSharedCollectionRepository todoSharedCollectionRepository, ITodoCollectionService todoCollectionService, 
    IUserContext userContext, IUnitOfWork unitOfWork) : ITodoSharedCollectionService
{
    public async Task<List<TodoSharedCollectionDto>> GetTodoSharedCollectionAsync(Guid todoCollectionId, CancellationToken cancellationToken)
    {
        var todoSharedCollections = await todoSharedCollectionRepository.GetAsync(todoCollectionId, cancellationToken);

        return todoSharedCollections.ToDtos();
    }
    
    public async Task<TodoSharedCollectionDto> ShareTodoCollectionAsync(TodoSharedCollectionDto todoSharedCollectionDto, CancellationToken cancellationToken)
    {
        await todoCollectionService.GetByIdAsync(todoSharedCollectionDto.TodoCollectionId, cancellationToken);
        
        var existingSharedCollection = await todoSharedCollectionRepository.GetAsync(todoSharedCollectionDto.TodoCollectionId, cancellationToken);
        
        if (existingSharedCollection is not null &&
            existingSharedCollection.Count >= Constants.ShareTodoCollectionLimit)
            throw new BadRequestException($"You can only share a collection with {Constants.ShareTodoCollectionLimit} users at a time.");
        
        var entity = todoSharedCollectionDto.ToEntity();
        
        var todoSharedCollection = await todoSharedCollectionRepository.CreateAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return todoSharedCollection.ToDto();
    }

    public async Task RevokeTodoCollectionAccessAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await todoSharedCollectionRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoSharedCollection), id);
        
        if (userContext.UserId != entity.TodoCollection.OwnerId)
            throw new ForbiddenException("You do not have permission to revoke access to this collection.");
        
        todoSharedCollectionRepository.Delete(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}