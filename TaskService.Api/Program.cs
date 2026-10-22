using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TaskService.Api.Data;
using TaskService.Api.Repositories;
using TaskService.Core.Repository;
using TaskServiceManager = TaskService.Core.TaskService;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();
builder.Services.AddScoped<TaskServiceManager>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapControllers();
app.MapHealthChecks("/health");
app.Urls.Add("http://0.0.0.0:80");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    db.Database.Migrate();
}

app.Run();