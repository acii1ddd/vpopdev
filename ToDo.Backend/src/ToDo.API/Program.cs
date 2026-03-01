using ToDo.API.ConfigurationExtensions;
using ToDo.API.EndpointSettings;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpoints(typeof(Program).Assembly)
    .AddRepositories()
    .AddDbContexts(builder.Configuration)
    .AddApiVersionControl()
    .AddMediatr()
    .AddJwtAuth(builder.Configuration)
    .AddOpenApi();

var app = builder.Build();

await app.ConfigureApp();

await app.RunAsync();