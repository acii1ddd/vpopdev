using ToDo.API.Data.Models.Entities;

namespace ToDo.API.Data.Interfaces;

public interface IToDoItemRepository
{
    public Task<IEnumerable<ToDoItem>> GetAllAsync(CancellationToken ct);
}