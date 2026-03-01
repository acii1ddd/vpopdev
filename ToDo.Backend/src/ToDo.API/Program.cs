using ToDo.API.ConfigurationExtensions;
using ToDo.API.EndpointSettings;
using ToDO.Shared.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpoints(typeof(Program).Assembly)
    .AddExceptionHandler<CustomExceptionHandler>()
    .AddRepositories()
    .AddDbContexts(builder.Configuration)
    .AddApiVersionControl()
    .AddMediatr()
    .AddJwtAuth(builder.Configuration)
    .AddOpenApi();

var app = builder.Build();

await app.ConfigureApp();

await app.RunAsync();