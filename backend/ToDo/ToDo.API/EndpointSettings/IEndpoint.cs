namespace ToDo.API.EndpointSettings;

public interface IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app);
}