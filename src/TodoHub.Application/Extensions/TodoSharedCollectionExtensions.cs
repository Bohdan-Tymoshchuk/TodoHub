using TodoHub.Application.DTOs;
using TodoHub.Domain.Entities;

namespace TodoHub.Application.Extensions;

public static class TodoSharedCollectionExtensions
{
    public static TodoSharedCollection ToEntity(this TodoSharedCollectionDto dto)
    {
        return new TodoSharedCollection
        {
            UserId = dto.UserId,
            TodoCollectionId = dto.TodoCollectionId
        };
    }
    
    public static TodoSharedCollectionDto ToDto(this TodoSharedCollection entity)
    {
        return new TodoSharedCollectionDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            TodoCollectionId = entity.TodoCollectionId
        };
    }
    
    public static List<TodoSharedCollectionDto> ToDtos(this IEnumerable<TodoSharedCollection> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}