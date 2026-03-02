using ToDo.API.Data.Models.Enums;

namespace ToDo.API.Data.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public Roles Role { get; set; } = Roles.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ToDoItem> ToDoItems { get; set; } = [];
}