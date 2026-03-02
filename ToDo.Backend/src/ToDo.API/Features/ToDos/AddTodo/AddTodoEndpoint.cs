using System.Security.Claims;

using Asp.Versioning;
using Asp.Versioning.Builder;

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
        ApiVersionSet versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder group = app.MapGroup(RoutePrefix)
            .WithApiVersionSet(versionSet)
            .HasApiVersion(new ApiVersion(1, 0));

        group.MapPost("/", async (
                ISender sender,
                [FromBody] AddToDoRequest request,
                ClaimsPrincipal user,
                CancellationToken ct) =>
            {
                string? userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                {
                    throw new UnauthorizedAccessException("User not authenticated");
                }

                AddTodoCommand command = new(request.Title, request.Description, request.Priority, Guid.Parse(userId));

                AddTodoResult result = await sender.Send(command, ct);

                return Results.Created($"/todos/{result.Item.Id}", result);
            })
            .WithName("AddTodoV1")
            .WithOpenApi()
            .RequireAuthorization("User");
    }
}