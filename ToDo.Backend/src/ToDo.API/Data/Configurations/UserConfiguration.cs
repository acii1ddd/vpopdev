using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo.API.Data.Models.Entities;

namespace ToDo.API.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private const int MaxLength = 256;
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(MaxLength);
        
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(MaxLength);

        // todo enum
        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(MaxLength);

        builder.Property(u => u.CreatedAt)
            .IsRequired();
        
        builder.HasMany(x => x.ToDoItems)
            .WithOne(x => x.User);

        builder.HasData(new List<User>
        {
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Alice Johnson",
                Email = "alice@example.com",
                PasswordHash = "$2a$11$DnMyYgwiyLwkC3iwUsXCOemxG5RlwWxNiKMLdRd75s/xVps2lA.gu",
                Role = "Admin",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc)
            },
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Bob Smith",
                Email = "bob@example.com",
                PasswordHash = "$2a$11$DnMyYgwiyLwkC3iwUsXCOemxG5RlwWxNiKMLdRd75s/xVps2lA.gu",
                Role = "User",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc)
            },
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Charlie Brown",
                Email = "charlie@example.com",
                PasswordHash = "$2a$11$DnMyYgwiyLwkC3iwUsXCOemxG5RlwWxNiKMLdRd75s/xVps2lA.gu",
                Role = "User",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc)
            },
            new()
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "Diana Prince",
                Email = "diana@example.com",
                PasswordHash = "$2a$11$DnMyYgwiyLwkC3iwUsXCOemxG5RlwWxNiKMLdRd75s/xVps2lA.gu",
                Role = "User",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc)
            },
            new()
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "Evan Wright",
                Email = "evan@example.com",
                PasswordHash = "$2a$11$DnMyYgwiyLwkC3iwUsXCOemxG5RlwWxNiKMLdRd75s/xVps2lA.gu",
                Role = "User",
                CreatedAt = DateTime.SpecifyKind(new DateTime(2025, 1, 1), DateTimeKind.Utc)
            }
        });
    }
}