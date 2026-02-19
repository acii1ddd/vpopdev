using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;

namespace ToDo.API.ConfigurationExtensions;

public static class DatabaseExtensions
{
    public static async Task InitDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredKeyedService<AppDbContext>("Write")
            ?? throw new InvalidOperationException("AppDbContext not found in service provider.");

        await context.Database.MigrateAsync();
    }
}