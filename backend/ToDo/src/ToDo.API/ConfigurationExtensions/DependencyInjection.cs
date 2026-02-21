using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;
using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Repositories;

namespace ToDo.API.ConfigurationExtensions;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRepositories()
        {
            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        
            return services;
        }

        public IServiceCollection AddDbContexts(IConfiguration configuration)
        {
            var writeConnection = configuration.GetConnectionString("WriteConnection");
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(writeConnection);
            });
            
            services.AddKeyedScoped<AppDbContext>("Read", (_, _) =>
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(configuration.GetConnectionString("ReadConnection"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .Options;
            
                return new AppDbContext(options);
            });
            
            return services;
        }
    }
}