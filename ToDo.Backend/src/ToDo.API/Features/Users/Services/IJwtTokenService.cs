using ToDo.API.Data.Models.Entities;

namespace ToDo.API.Features.Users.Services;

public interface IJwtTokenService
{
    public string GenerateToken(User user);
}
