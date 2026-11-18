using backend.Data;
using backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Строка подключения Default не найдена.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IAppInfoService, AppInfoService>();
builder.Services.AddScoped<IRepairRequestService, RepairRequestService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok("ok"));

app.MapGet("/info", (IAppInfoService appInfo) => new
{
    application = appInfo.GetApplicationName(),
    environment = app.Environment.EnvironmentName,
    version = "Лабораторная работа 10"
});

app.UseAuthorization();
app.MapControllers();

app.Run();
