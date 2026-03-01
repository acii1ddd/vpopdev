using Microsoft.EntityFrameworkCore;
using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;

namespace ToDo.API.Data.Repositories;

public class ToDoItemRepository([FromKeyedServices("Read")]AppDbContext readDbContext, AppDbContext writeDbContext) 
    : IToDoItemRepository
{
    public async Task<IEnumerable<ToDoItem>> GetAllAsync(CancellationToken ct)
    {
        return await readDbContext.ToDoItems
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<ToDoItem?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await readDbContext.ToDoItems
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task DeleteAsync(ToDoItem todoResult, CancellationToken ct)
    {
        writeDbContext.ToDoItems.Remove(todoResult);
        
        await writeDbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ToDoItem todo, CancellationToken ct)
    {
        writeDbContext.Update(todo);
        
        await writeDbContext.SaveChangesAsync(ct);
    }

    public async Task AddAsync(ToDoItem todo, CancellationToken ct)
    {
        await writeDbContext.ToDoItems.AddAsync(todo, ct);
        
        await writeDbContext.SaveChangesAsync(ct);
    }
}
