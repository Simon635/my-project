using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IAppInfoService, AppInfoService>();
builder.Services.AddSingleton<IRepairRequestService, RepairRequestService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Проверка доступности приложения
app.MapGet("/health", () => Results.Ok("ok"));

// Информация о приложении
app.MapGet("/info", (IAppInfoService appInfo) => Results.Ok(new
{
    application = appInfo.GetApplicationName(),
    environment = app.Environment.EnvironmentName,
    version = "Лабораторная работа 7"
}));

app.UseAuthorization();

app.MapControllers();

app.Run();
