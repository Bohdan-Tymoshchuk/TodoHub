using TodoHub.Application.DTOs;

namespace TodoHub.Application.Services.Abstractions;

public interface IUserService
{
    public Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken);
    
    public Task<UserDto> CreateAsync(UserDto userDto, CancellationToken cancellationToken);
}