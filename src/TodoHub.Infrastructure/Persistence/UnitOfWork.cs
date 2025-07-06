using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using TodoHub.Domain.Abstractions;

namespace TodoHub.Infrastructure.Persistence;

public class UnitOfWork(TodoDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
