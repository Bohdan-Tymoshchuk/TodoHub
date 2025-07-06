using TodoHub.Domain.Entities;

namespace TodoHub.Domain.Abstractions.Repositories;

public interface ITodoSharedCollectionRepository
{
    Task<TodoSharedCollection?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<List<TodoSharedCollection>> GetAsync(Guid todoCollectionId, CancellationToken cancellationToken);
    
    Task<TodoSharedCollection> CreateAsync(TodoSharedCollection todoSharedCollection, CancellationToken cancellationToken);
    
    void Delete(TodoSharedCollection todoSharedCollection);
}