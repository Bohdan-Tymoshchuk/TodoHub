using TodoHub.Domain.Entities;
using TodoHub.Domain.Pagination;

namespace TodoHub.Domain.Abstractions.Repositories;

public interface ITodoCollectionRepository
{
    Task<TodoCollection?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    
    Task<TodoCollection?> GetByIdForOwnerOnlyAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

    public Task<PaginatedResult<TodoCollection>> GetAllAsync(Guid userId, PaginationRequest request, CancellationToken cancellationToken = default);
    
    Task<TodoCollection> CreateAsync(TodoCollection todoCollection, CancellationToken cancellationToken = default);
    
    TodoCollection Update(TodoCollection todoCollection);
    
    void Delete(TodoCollection todoCollection);
}