using Asp.Versioning;
using MediatR;
using ToDo.API.EndpointSettings;

namespace ToDo.API.Features.ToDos.GetTodos;

public class GetToDosV1Endpoint : IEndpoint
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

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetTodosQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllTodosV1")
        .WithOpenApi()
        .RequireAuthorization("User");
    }
}