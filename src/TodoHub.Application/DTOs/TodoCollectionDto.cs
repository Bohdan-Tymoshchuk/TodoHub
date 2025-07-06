namespace TodoHub.Application.DTOs;

public class TodoCollectionDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
    
    public List<TodoTaskDto> Tasks { get; set; } = [];
}