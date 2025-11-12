using Backed.Domain.Interfaces;
using Backed.Infrastructure.Data;
using Backed.Infrastructure.Services;
using Backed.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(op =>
{
    op.Interceptors.Add<JwtInterceptor>();
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5101, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddSingleton<JwtService>(provider =>
{
    var config = builder.Configuration;
    return new JwtService(
        config["Jwt:Key"] ?? "default_secret_key_that_should_be_changed_in_production",
        config["Jwt:Issuer"] ?? "WPFAdmin",
        config["Jwt:Audience"] ?? "WPFAdminUsers",
        int.Parse(config["Jwt:ExpiresInMinutes"] ?? "120")
    );
});

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<RBACService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();