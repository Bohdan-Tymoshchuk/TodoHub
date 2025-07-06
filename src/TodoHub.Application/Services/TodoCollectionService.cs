using BuildingBlocks.Exceptions;
using TodoHub.Application.DTOs;
using TodoHub.Application.Extensions;
using TodoHub.Application.Services.Abstractions;
using TodoHub.Domain.Abstractions;
using TodoHub.Domain.Abstractions.Repositories;
using TodoHub.Domain.Contexts;
using TodoHub.Domain.Entities;
using TodoHub.Domain.Pagination;

namespace TodoHub.Application.Services;

public class TodoCollectionService(ITodoCollectionRepository todoCollectionRepository, IUnitOfWork unitOfWork, IUserContext userContext) : ITodoCollectionService
{
    public async Task<TodoCollectionDto> CreateAsync(TodoCollectionDto todoCollectionDto, CancellationToken cancellationToken = default)
    {
        var entity = todoCollectionDto.ToEntity(userContext.UserId);
        
        var todoCollection = await todoCollectionRepository.CreateAsync(entity, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return todoCollection.ToDto();
    }

    public async Task<TodoCollectionDto> UpdateAsync(Guid id, TodoCollectionDto todoCollectionDto, CancellationToken cancellationToken = default)
    {
        var entity = await todoCollectionRepository.GetByIdAsync(id, userContext.UserId, cancellationToken)
                     ?? throw new NotFoundException(nameof(TodoCollection), id);
        
        var updatedEntity = todoCollectionDto.ToEntity(entity);
        var todoCollection = todoCollectionRepository.Update(updatedEntity);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return todoCollection.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await todoCollectionRepository.GetByIdForOwnerOnlyAsync(id, userContext.UserId, cancellationToken)
                     ?? throw new NotFoundException(nameof(TodoCollection), id);

        todoCollectionRepository.Delete(entity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<TodoCollectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var todoCollection = await todoCollectionRepository.GetByIdAsync(id, userContext.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(TodoCollection), id);

        return todoCollection.ToDto();
    }

    public async Task<PaginatedResult<TodoCollectionDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        var todoCollections = await todoCollectionRepository.GetAllAsync(userContext.UserId, paginationRequest, cancellationToken);

        return todoCollections.ToDtos();
    }
}