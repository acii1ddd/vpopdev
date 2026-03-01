using ToDo.API.Data.Models.Entities;

namespace ToDo.API.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
