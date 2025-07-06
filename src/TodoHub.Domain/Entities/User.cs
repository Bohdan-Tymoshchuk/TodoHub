namespace TodoHub.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public ICollection<TodoCollection> TodoCollections { get; set; } = new List<TodoCollection>();
    
    public ICollection<TodoSharedCollection> TodoSharedCollections { get; set; } = new List<TodoSharedCollection>();
}