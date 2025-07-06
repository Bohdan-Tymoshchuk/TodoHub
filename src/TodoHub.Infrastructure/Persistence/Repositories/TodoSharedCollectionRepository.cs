using Microsoft.EntityFrameworkCore;
using TodoHub.Domain.Abstractions.Repositories;
using TodoHub.Domain.Entities;

namespace TodoHub.Infrastructure.Persistence.Repositories;

public class TodoSharedCollectionRepository(TodoDbContext dbContext) : ITodoSharedCollectionRepository
{
    public async Task<TodoSharedCollection?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.TodoSharedCollections
            .Include(x => x.TodoCollection)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<TodoSharedCollection>> GetAsync(Guid todoCollectionId, CancellationToken cancellationToken)
    {
        return await dbContext.TodoSharedCollections
            .Where(x => x.TodoCollectionId == todoCollectionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<TodoSharedCollection> CreateAsync(TodoSharedCollection todoSharedCollection, CancellationToken cancellationToken)
    {
        var result = await dbContext.TodoSharedCollections.AddAsync(todoSharedCollection, cancellationToken);
        
        return result.Entity;
    }

    public void Delete(TodoSharedCollection todoSharedCollection)
    {
        dbContext.TodoSharedCollections.Remove(todoSharedCollection);
    }
}