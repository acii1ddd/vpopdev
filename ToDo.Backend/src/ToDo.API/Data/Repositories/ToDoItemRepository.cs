using Microsoft.EntityFrameworkCore;
using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Models.Entities;

namespace ToDo.API.Data.Repositories;

public class ToDoItemRepository([FromKeyedServices("Read")]AppDbContext appDbContext) 
    : IToDoItemRepository
{
    public async Task<IEnumerable<ToDoItem>> GetAllAsync(CancellationToken ct)
    {
        return await appDbContext.ToDoItems
            .AsNoTracking()
            .ToListAsync(ct);
    }
}