using ToDo.API.Data.Models.Entities;

namespace ToDo.API.Data.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    public Task AddAsync(User user, CancellationToken ct);
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
    public Task<IEnumerable<User>> GetAllAsync(CancellationToken ct);
    public Task DeleteAsync(User user, CancellationToken ct);
    public Task UpdateAsync(User user, CancellationToken ct);
}
