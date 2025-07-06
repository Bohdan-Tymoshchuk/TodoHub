using Microsoft.EntityFrameworkCore;
using TodoHub.Domain.Entities;
using TodoHub.Infrastructure.Persistence.Extensions;

namespace TodoHub.Infrastructure.Persistence;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<TodoTask> TodoTasks { get; set; }
    
    public DbSet<TodoCollection> TodoCollections { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<TodoSharedCollection> TodoSharedCollections { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ConfigureTodoTaskEntity()
            .ConfigureTodoCollectionEntity()
            .ConfigureUserEntity()
            .ConfigureTodoSharedCollectionEntity();
    }
}