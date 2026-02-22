using Asp.Versioning;

namespace ToDo.API.EndpointSettings;

public interface IEndpoint
{
    public ApiVersion ApiVersion => new(1, 0);

    public string RoutePrefix => string.Empty;

    public void MapEndpoint(IEndpointRouteBuilder app);
}