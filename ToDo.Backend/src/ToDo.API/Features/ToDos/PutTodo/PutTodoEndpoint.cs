using Asp.Versioning;
using Asp.Versioning.Builder;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using ToDo.API.Dtos.ToDo;
using ToDo.API.EndpointSettings;

namespace ToDo.API.Features.ToDos.PutTodo;

public class PutTodoEndpoint : IEndpoint
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

        group.MapPut("/{id:guid}", async (
                ISender sender,
                [FromRoute] Guid id,
                [FromBody] PutToDoRequest request,
                CancellationToken ct) =>
            {
                PutTodoCommand command = new(id, request.Title, request.Description, request.IsCompleted,
                    request.Priority);

                PutTodoResult result = await sender.Send(command, ct);

                return Results.Ok(result);
            })
            .WithName("PutTodoV1")
            .WithOpenApi()
            .RequireAuthorization("User");
    }
}