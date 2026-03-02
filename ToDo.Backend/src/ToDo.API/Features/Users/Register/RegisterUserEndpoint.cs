using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Dtos.Auth;
using ToDo.API.EndpointSettings;

namespace ToDo.API.Features.Users.Register;

public class RegisterUserEndpoint : IEndpoint
{
    public ApiVersion ApiVersion => new(1, 0);

    public string RoutePrefix => "register";
    
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
                [FromBody] RegisterRequest request, 
                CancellationToken ct) =>
            {
                var command = new RegisterUserCommand(request.Name, request.Email, request.Password);
            
                await sender.Send(command, ct);
            
                return Results.Ok();
            })
            .WithName("RegisterUserV1")
            .WithOpenApi()
            .AllowAnonymous();
    }
}