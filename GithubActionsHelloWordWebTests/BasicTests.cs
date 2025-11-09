using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace GithubActionsHelloWordWebTests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_RootEndpoint_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
    response.EnsureSuccessStatusCode();
   Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType?.ToString());
   
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello World from ASP.NET Core!", content);
    }

    [Fact]
    public async Task Get_ApiHelloEndpoint_ReturnsSuccessAndCorrectJson()
    {
        // Act
        var response = await _client.GetAsync("/api/hello");

      // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

        var content = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
     
        Assert.True(jsonDoc.RootElement.TryGetProperty("message", out var messageProperty));
        Assert.Equal("Hello from API!", messageProperty.GetString());
 
        Assert.True(jsonDoc.RootElement.TryGetProperty("timestamp", out var timestampProperty));
        Assert.True(DateTime.TryParse(timestampProperty.GetString(), out _));
    }

    [Fact]
    public async Task Get_NonExistentEndpoint_ReturnsNotFound()
    {
  // Act
     var response = await _client.GetAsync("/nonexistent");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}