namespace ToDo.API.Data.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateOnly CreatedAt { get; set; }
    public ICollection<ToDoItem> ToDoItems { get; set; } = [];
}