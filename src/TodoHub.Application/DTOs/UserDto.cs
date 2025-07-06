namespace TodoHub.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string Email { get; set; }
}