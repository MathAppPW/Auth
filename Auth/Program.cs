using Auth.Dal.Interfaces;
using Auth.Helpers;
using Auth.Helpers.Extensions;
using Auth.Services;
using Microsoft.EntityFrameworkCore;
using Grpc.AspNetCore.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var connectionString = "Server=mathapp-tests.postgres.database.azure.com;Database=postgres;Port=5432;User Id=mathapp;Password=projektZespolowy123;Ssl Mode=Require;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AuthenticatorService>();
app.MapGrpcReflectionService();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();