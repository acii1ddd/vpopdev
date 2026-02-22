using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;

namespace ToDo.API.ConfigurationExtensions;

public static class DatabaseExtensions
{
    public static async Task InitDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        await context.Database.MigrateAsync();
    }
}