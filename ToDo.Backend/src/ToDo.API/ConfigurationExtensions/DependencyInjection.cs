using Asp.Versioning;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using ToDo.API.Data;
using ToDo.API.Data.Interfaces;
using ToDo.API.Data.Repositories;
using ToDo.API.Features.ToDos.AddTodo;
using ToDo.API.Features.ToDos.GetTodos;
using ToDo.API.Features.Users.Services;
using ToDo.Shared.Behaviors;

namespace ToDo.API.ConfigurationExtensions;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRepositories()
        {
            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            return services;
        }

        public IServiceCollection AddDbContexts(IConfiguration configuration)
        {
            string? writeConnection = configuration.GetConnectionString("WriteConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(writeConnection);
            });

            services.AddKeyedScoped<AppDbContext>("Read", (_, _) =>
            {
                DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(configuration.GetConnectionString("ReadConnection"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                    .Options;

                return new AppDbContext(options);
            });

            return services;
        }

        public IServiceCollection AddApiVersionControl()
        {
            services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("x-api-version"),
                        new QueryStringApiVersionReader("api-version")
                    );
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            return services;
        }


        public IServiceCollection AddMediatr()
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(GetTodosQueryHandler).Assembly);

                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(AddTodoCommandValidator).Assembly);

            return services;
        }
    }
}