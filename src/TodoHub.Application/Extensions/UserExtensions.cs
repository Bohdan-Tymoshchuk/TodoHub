using TodoHub.Application.DTOs;
using TodoHub.Domain.Entities;

namespace TodoHub.Application.Extensions;

public static class UserExtensions
{
    public static User ToEntity(this UserDto dto)
    {
        return new User
        {
            Name = dto.Name,
            Email = dto.Email
        };
    }
    
    public static UserDto ToDto(this User entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email
        };
    }
    
    public static List<UserDto> ToDtos(this IEnumerable<User> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}