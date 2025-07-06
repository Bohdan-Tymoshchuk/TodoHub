using TodoHub.Application.DTOs;
using TodoHub.Application.Extensions;
using TodoHub.Application.Services.Abstractions;
using TodoHub.Domain.Abstractions;
using TodoHub.Domain.Abstractions.Repositories;

namespace TodoHub.Application.Services;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(cancellationToken);
        
        return users.ToDtos();
    }

    public async Task<UserDto> CreateAsync(UserDto userDto, CancellationToken cancellationToken)
    {
        var user = await userRepository.CreateAsync(userDto.ToEntity(), cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
       
        return user.ToDto();
    }
}