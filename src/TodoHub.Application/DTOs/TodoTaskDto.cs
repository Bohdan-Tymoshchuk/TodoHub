namespace TodoHub.Application.DTOs;

public class TodoTaskDto
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }
}