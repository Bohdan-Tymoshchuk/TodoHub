using TodoHub.Application.DTOs;
using TodoHub.Domain.Entities;
using TodoHub.Domain.Pagination;

namespace TodoHub.Application.Extensions;

public static class TodoCollectionExtensions
{
    public static TodoCollection ToEntity(this TodoCollectionDto dto, Guid ownerId)
    {
        return new TodoCollection
        {
            Name = dto.Name,
            OwnerId = ownerId,
            TodoTasks = dto.Tasks.Select(task => new TodoTask
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            }).ToList()
        };
    }
    
    public static TodoCollection ToEntity(this TodoCollectionDto dto, TodoCollection existingEntity)
    {
        existingEntity.Name = dto.Name;
        existingEntity.TodoTasks = dto.Tasks.Select(task => new TodoTask
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted
        }).ToList();

        return existingEntity;
    }
    
    public static TodoCollectionDto ToDto(this TodoCollection entity)
    {
        return new TodoCollectionDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Tasks = entity.TodoTasks.Select(task => new TodoTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            }).ToList()
        };
    }
    
    public static PaginatedResult<TodoCollectionDto> ToDtos(this PaginatedResult<TodoCollection> entities)
    {
        return new PaginatedResult<TodoCollectionDto>(
            entities.PageIndex,
            entities.PageSize,
            entities.TotalCount,
            entities.Items.Select(ToDto).ToList()
        );
    }
}