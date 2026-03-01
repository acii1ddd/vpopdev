using Asp.Versioning;
using MediatR;
using ToDo.API.EndpointSettings;

namespace ToDo.API.Features.ToDos.DeleteTodo;

// todo test with 11111111-1111-1111-1111-111111111111 parameter and fix handling exeption
public class DeleteTodoEndpoint : IEndpoint
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

        group.MapDelete("/{id:guid}", async (ISender sender, Guid id, CancellationToken ct) =>
        {
            var result = await sender.Send(new DeleteTodoCommand(id), ct);
            return Results.Ok(result);
        })
        .WithName("DeleteTodoV1")
        .WithOpenApi();
    }
}