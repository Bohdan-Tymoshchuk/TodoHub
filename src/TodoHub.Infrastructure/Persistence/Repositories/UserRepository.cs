using Microsoft.EntityFrameworkCore;
using TodoHub.Domain.Abstractions.Repositories;
using TodoHub.Domain.Entities;

namespace TodoHub.Infrastructure.Persistence.Repositories;

public class UserRepository(TodoDbContext dbContext) : IUserRepository
{
    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Users.ToListAsync(cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
    {
        var result = await dbContext.Users.AddAsync(user, cancellationToken);
       
        return result.Entity;
    }
}