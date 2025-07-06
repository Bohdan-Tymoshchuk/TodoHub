using TodoHub.Application.DTOs;

namespace TodoHub.Application.Services.Abstractions;

public interface ITodoSharedCollectionService
{
    Task<List<TodoSharedCollectionDto>> GetTodoSharedCollectionAsync(Guid todoCollectionId, CancellationToken cancellationToken);
    
    Task<TodoSharedCollectionDto> ShareTodoCollectionAsync(TodoSharedCollectionDto todoSharedCollectionDto, CancellationToken cancellationToken);
    
    Task RevokeTodoCollectionAccessAsync(Guid id, CancellationToken cancellationToken);
}