using Auth.Dal.Interfaces;
using Auth.Helpers;
using Auth.Helpers.Extensions;
using Auth.Services;
using Auth.Dal;
using Auth.Dal.Extensions;
using Microsoft.EntityFrameworkCore;
using Grpc.AspNetCore.Server;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5080, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>{});

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddServices();
builder.Services.AddRepos();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AuthenticatorService>();
app.MapGrpcReflectionService();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();