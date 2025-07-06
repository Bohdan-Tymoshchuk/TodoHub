using TodoHub.API;
using TodoHub.API.Endpoints;
using TodoHub.Application;
using TodoHub.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

app.UseApiServices();

app.MapTodoCollectionEndpoints();
app.MapUserEndpoints();
app.MapTodoSharedCollectionEndpoints();

app.Run();