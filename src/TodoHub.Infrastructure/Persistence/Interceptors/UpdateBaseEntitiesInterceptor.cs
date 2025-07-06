using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoHub.Domain.Entities;

namespace TodoHub.Infrastructure.Persistence.Interceptors;

public class UpdateBaseEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = dbContext.ChangeTracker.Entries<BaseEntity>();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
                entityEntry.Entity.Id = Guid.NewGuid();
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}