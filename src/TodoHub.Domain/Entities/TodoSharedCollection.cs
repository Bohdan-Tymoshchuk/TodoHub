namespace TodoHub.Domain.Entities;

public class TodoSharedCollection : AuditableEntity
{
    public Guid UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    public Guid TodoCollectionId { get; set; }
        
    public TodoCollection TodoCollection { get; set; } = null!;
}