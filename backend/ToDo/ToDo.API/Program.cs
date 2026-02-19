using ToDo.API.ConfigurationExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRepositories()
    .AddDbContexts(builder.Configuration);

var app = builder.Build();

await app.ConfigureApp();

await app.RunAsync();