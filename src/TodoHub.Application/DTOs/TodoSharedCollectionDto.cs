namespace TodoHub.Application.DTOs;

public class TodoSharedCollectionDto
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid TodoCollectionId { get; set; }
}