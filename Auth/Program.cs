using Auth.Helpers.Extensions;
using Auth.Services;
using Auth.Dal;
using Auth.Dal.Extensions;
using Auth.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();

var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>("Auth", jwtSection.GetSection("Auth"));
builder.Services.Configure<JwtSettings>("Refresh", jwtSection.GetSection("Refresh"));


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
