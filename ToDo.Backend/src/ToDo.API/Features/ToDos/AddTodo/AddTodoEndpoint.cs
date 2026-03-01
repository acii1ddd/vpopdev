using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Dtos.ToDo;
using ToDo.API.EndpointSettings;

namespace ToDo.API.Features.ToDos.AddTodo;

public class AddTodoEndpoint : IEndpoint
{
    public ApiVersion ApiVersion => new(1, 0);

    public string RoutePrefix => "todos";
    
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(RoutePrefix)
            .WithApiVersionSet(versionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        group.MapPost("/", async (
            ISender sender, 
            [FromBody] AddToDoRequest request, 
            CancellationToken ct) =>
        {
            var command = new AddTodoCommand(request.Title, request.Description, request.Priority);
            
            var result = await sender.Send(command, ct);
            
            return Results.Created($"/todos/{result.Item.Id}", result);
        })
        .WithName("AddTodoV1")
        .WithOpenApi();
    }
}
