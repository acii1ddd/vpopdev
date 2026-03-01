namespace ToDo.API.Data.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public string Role { get; set; } = "User"; // todo enum

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ToDoItem> ToDoItems { get; set; } = [];
}