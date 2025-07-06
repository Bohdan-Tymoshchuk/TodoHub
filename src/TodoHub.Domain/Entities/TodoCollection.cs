namespace TodoHub.Domain.Entities;

public class TodoCollection : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid OwnerId { get; set; }
    
    public User Owner { get; set; } = null!;

    public ICollection<TodoTask> TodoTasks { get; set; } = [];
    
    public ICollection<TodoSharedCollection> TodoSharedCollections { get; set; } = [];
}