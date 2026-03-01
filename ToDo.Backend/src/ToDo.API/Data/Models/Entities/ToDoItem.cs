using ToDo.API.Data.Models.Enums;

namespace ToDo.API.Data.Models.Entities;

public class ToDoItem
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Priority Priority { get; set; } = Priority.Medium;

    public User User { get; set; } = null!;
}