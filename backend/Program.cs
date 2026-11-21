using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("VueClient", policy =>
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Строка подключения Default не найдена.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IAppInfoService, AppInfoService>();
builder.Services.AddScoped<IRepairRequestService, RepairRequestService>();
builder.Services.AddScoped<PasswordHasher<User>>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "MultimediaRepair.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;

        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Учебные аккаунты: хеши создаются при первом запуске.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<User>>();

    foreach (var (login, password) in new[]
    {
        ("dispatcher", "Dispatcher123!"),
        ("technician", "Technician123!")
    })
    {
        var user = db.Users.Single(x => x.Login == login);

        if (string.IsNullOrEmpty(user.PasswordHash))
            user.PasswordHash = hasher.HashPassword(user, password);
    }

    db.SaveChanges();
}

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
    version = "Лабораторная работа 11"
});

app.UseCors("VueClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
