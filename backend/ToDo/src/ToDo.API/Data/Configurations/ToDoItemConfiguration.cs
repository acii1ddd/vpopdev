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
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        builder.Property(t => t.Priority)
            .HasDefaultValue(Priority.Medium);
        
        builder.HasOne(t => t.User)
            .WithMany(u => u.ToDoItems);

        builder.HasData(
        [
            new ToDoItem
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Alice
                Title = "Купить продукты",
                Description = "Молоко, хлеб, яйца",
                IsCompleted = false,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Priority = Priority.Medium
            },
            new ToDoItem
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Alice
                Title = "Завершить отчет",
                Description = "Отчет за квартал",
                IsCompleted = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Priority = Priority.High
            },
            new ToDoItem
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), // Bob
                Title = "Сходить в спортзал",
                Description = "Тренировка ног",
                IsCompleted = false,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Priority = Priority.Low
            },
            new ToDoItem
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"), // Charlie
                Title = "Прочитать книгу",
                Description = "Главы 1-5",
                IsCompleted = false,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Priority = Priority.Medium
            },
            new ToDoItem
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"), // Diana
                Title = "Позвонить клиенту",
                Description = "Обсудить детали проекта",
                IsCompleted = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Priority = Priority.High
            }
        ]);
    }
}