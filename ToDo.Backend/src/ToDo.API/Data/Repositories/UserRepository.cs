using Microsoft.EntityFrameworkCore;
using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;

namespace ToDo.API.Data.Repositories;

public class UserRepository([FromKeyedServices("Read")] AppDbContext readDbContext, AppDbContext writeDbContext)
    : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await readDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await writeDbContext.Users.AddAsync(user, ct);
        await writeDbContext.SaveChangesAsync(ct);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await readDbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
    {
        return await readDbContext.Users
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task DeleteAsync(User user, CancellationToken ct)
    {
        writeDbContext.Users.Remove(user);
        await writeDbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct)
    {
        writeDbContext.Update(user);
        await writeDbContext.SaveChangesAsync(ct);
    }
}
