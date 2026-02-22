using Asp.Versioning;
using MediatR;
using ToDo.API.EndpointSettings;

namespace ToDo.API.Features.GetTodos;

public class GetToDosV2Endpoint : IEndpoint
{
    public ApiVersion ApiVersion => new(2, 0);

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
            .HasApiVersion(new ApiVersion(2, 0));

        group.MapGet("/", async (ISender sender, CancellationToken ct) =>
        {
            return Results.NotFound();
        })
        .WithName("GetAllTodosV2")
        .WithOpenApi();
    }
}
