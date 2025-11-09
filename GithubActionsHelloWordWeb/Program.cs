using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // This requires Swashbuckle.AspNetCore NuGet package

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World from ASP.NET Core!")
    .WithName("GetHello")
    .WithOpenApi();

app.MapGet("/api/hello", () => new { Message = "Hello from API!", Timestamp = DateTime.UtcNow })
    .WithName("GetHelloApi")
    .WithOpenApi();

app.Run();

// Make the Program class public for testing
public partial class Program
{ }