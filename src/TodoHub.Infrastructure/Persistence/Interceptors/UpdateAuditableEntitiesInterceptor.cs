using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoHub.Domain.Contexts;
using TodoHub.Domain.Entities;

namespace TodoHub.Infrastructure.Persistence.Interceptors;

public class UpdateAuditableEntitiesInterceptor(IUserContext userContext) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = dbContext.ChangeTracker.Entries<AuditableEntity>();

        foreach (var entityEntry in entries)
        {
            entityEntry.Entity.LastUpdated = DateTime.UtcNow;
            entityEntry.Entity.LastUpdatedBy = userContext.UserId;

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedDate = DateTime.UtcNow;
                entityEntry.Entity.CreatedBy = userContext.UserId;
            }
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
