using TodoHub.Domain.Contexts;

namespace TodoHub.API.Contexts;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        Guid.TryParse(httpContextAccessor.HttpContext?.Request.Headers["x-user-id"].FirstOrDefault(), out var userId)
            ? userId
            : Guid.Empty;
}