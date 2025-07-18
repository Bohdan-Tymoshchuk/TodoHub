namespace TodoHub.Domain.Entities;

public class TodoTask : AuditableEntity
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public Guid TodoCollectionId { get; set; }

    public TodoCollection TodoCollection { get; set; } = null!;
}