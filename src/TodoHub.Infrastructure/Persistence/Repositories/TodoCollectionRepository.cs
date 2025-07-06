using Microsoft.EntityFrameworkCore;
using TodoHub.Domain.Abstractions.Repositories;
using TodoHub.Domain.Entities;
using TodoHub.Domain.Pagination;

namespace TodoHub.Infrastructure.Persistence.Repositories;

public class TodoCollectionRepository(TodoDbContext dbContext) : ITodoCollectionRepository
{
    public Task<TodoCollection?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.TodoCollections
            .Where(x => x.OwnerId == userId || x.TodoSharedCollections.Any(s => s.UserId == userId))
            .Include(tc => tc.TodoTasks)
            .FirstOrDefaultAsync(tc => tc.Id == id, cancellationToken);
    }

    public Task<TodoCollection?> GetByIdForOwnerOnlyAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.TodoCollections
            .Where(x => x.OwnerId == userId)
            .Include(tc => tc.TodoTasks)
            .FirstOrDefaultAsync(tc => tc.Id == id, cancellationToken);
    }

    public async Task<PaginatedResult<TodoCollection>> GetAllAsync(Guid userId, PaginationRequest request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.TodoCollections
            .Where(x => x.OwnerId == userId || x.TodoSharedCollections.Any(s => s.UserId == userId))
            .OrderBy(tc => tc.Id)
            .Include(tc => tc.TodoTasks);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new PaginatedResult<TodoCollection>(request.PageIndex, request.PageSize, totalCount, items);
    }

    public async Task<TodoCollection> CreateAsync(TodoCollection todoCollection, CancellationToken cancellationToken = default)
    {
        var result = await dbContext.TodoCollections.AddAsync(todoCollection, cancellationToken);
        
        return result.Entity;
    }

    public TodoCollection Update(TodoCollection todoCollection)
    {
        var result = dbContext.TodoCollections.Update(todoCollection);
        
        return result.Entity;
    }

    public void Delete(TodoCollection todoCollection)
    {
        dbContext.TodoCollections.Remove(todoCollection);
    }
}