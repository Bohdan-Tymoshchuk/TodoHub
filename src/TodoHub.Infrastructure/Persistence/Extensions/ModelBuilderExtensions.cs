using Microsoft.EntityFrameworkCore;
using TodoHub.Domain.Entities;

namespace TodoHub.Infrastructure.Persistence.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ConfigureTodoTaskEntity(this ModelBuilder builder)
    {
        return builder.Entity<TodoTask>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(x => x.Description)
                .HasMaxLength(500);
            
            entity.HasOne(x => x.TodoCollection)
                .WithMany(x => x.TodoTasks)
                .HasForeignKey(x => x.TodoCollectionId);
        });
    }
    
    public static ModelBuilder ConfigureTodoCollectionEntity(this ModelBuilder builder)
    {
        return builder.Entity<TodoCollection>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            _ = entity.HasOne(x => x.Owner)
                .WithMany(x => x.TodoCollections)
                .HasForeignKey(x => x.OwnerId);
        });
    }
    
    public static ModelBuilder ConfigureUserEntity(this ModelBuilder builder)
    {
        return builder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            
            entity.Property(x => x.Name).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(255);
        });
    }
    
    public static ModelBuilder ConfigureTodoSharedCollectionEntity(this ModelBuilder builder)
    {
        return builder.Entity<TodoSharedCollection>(entity =>
        {
            entity.HasKey(x => x.Id);
            
            entity.HasOne(x => x.TodoCollection)
                .WithMany(x => x.TodoSharedCollections)
                .HasForeignKey(x => x.TodoCollectionId);
            entity.HasOne(x => x.User)
                .WithMany(x => x.TodoSharedCollections)
                .HasForeignKey(x => x.UserId);
        });
    }
}