using TodoHub.Domain.Entities;

namespace TodoHub.Domain.Abstractions.Repositories;

public interface IUserRepository
{
    public Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
    
    public Task<User> CreateAsync(User user, CancellationToken cancellationToken);
}