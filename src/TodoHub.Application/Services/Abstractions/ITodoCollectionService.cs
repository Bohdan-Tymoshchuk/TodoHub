using TodoHub.Application.DTOs;
using TodoHub.Domain.Pagination;

namespace TodoHub.Application.Services.Abstractions;

public interface ITodoCollectionService
{
    Task<TodoCollectionDto> CreateAsync(TodoCollectionDto todoCollectionDto, CancellationToken cancellationToken = default);

    Task<TodoCollectionDto> UpdateAsync(Guid id, TodoCollectionDto todoCollectionDto, CancellationToken cancellationToken = default);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<TodoCollectionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PaginatedResult<TodoCollectionDto>> GetAllAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
}