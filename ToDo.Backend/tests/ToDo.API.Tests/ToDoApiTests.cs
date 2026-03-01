using Microsoft.AspNetCore.Mvc.Testing;

namespace ToDo.API.Tests;

public class ToDoApiTests(WebApplicationFactory<Program> factory) 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public void Test1()
    {
        var response = _client.GetAsync("api/v1");

        // todo endpoint tests
        //response.EnsureSuccessStatusCode();
    }
}