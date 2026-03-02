using Asp.Versioning;
using Asp.Versioning.Builder;

using MediatR;

using ToDo.API.EndpointSettings;

namespace ToDo.API.Features.ToDos.DeleteTodo;

public class DeleteTodoEndpoint : IEndpoint
{
    public ApiVersion ApiVersion => new(1, 0);

    public string RoutePrefix => "todos";

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        ApiVersionSet versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder group = app.MapGroup(RoutePrefix)
            .WithApiVersionSet(versionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        group.MapDelete("/{id:guid}", async (
                ISender sender,
                Guid id,
                CancellationToken ct) =>
            {
                DeleteTodoResult result = await sender.Send(new DeleteTodoCommand(id), ct);

                return Results.Ok(result);
            })
            .WithName("DeleteTodoV1")
            .WithOpenApi()
            .RequireAuthorization("User");
    }
}