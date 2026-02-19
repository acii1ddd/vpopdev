using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.API.Data.Models.Entities;
using ToDo.API.Data.Models.Enums;

namespace ToDo.API.Data.Configurations;

public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItem>
{
    public void Configure(EntityTypeBuilder<ToDoItem> builder)
    {
        builder.ToTable("toDoItems");
        
        builder.HasKey(t => t.Id);

        builder.Property(t => t.UserId)
            .IsRequired();
        
        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.IsCompleted)
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasDefaultValue(DateOnly.FromDateTime(DateTime.UtcNow));
        
        builder.Property(t => t.UpdatedAt)
            .IsRequired()
            .HasDefaultValue(DateOnly.FromDateTime(DateTime.UtcNow));
        
        builder.Property(t => t.Priority)
            .HasDefaultValue(Priority.Medium);
        
        builder.HasOne(t => t.User)
            .WithMany(u => u.ToDoItems);
    }
}