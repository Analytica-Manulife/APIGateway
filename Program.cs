using ApiGateway.Data;
using ApiGateway.Repositories;
using ApiGateway.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)  
    .CreateLogger();

// Add services to the container.

// Register DbContext with SQL Server connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("FinanceNewsService", client =>
{
    client.BaseAddress = new Uri("https://news-app-analytica-hcdfg9achuazh7aa.canadacentral-01.azurewebsites.net/"); 
});

builder.Services.AddHttpClient("BudgetService", client =>
{
    client.BaseAddress = new Uri("https://bugdet-app-afc5a2hqbpgscsbs.canadacentral-01.azurewebsites.net/");
});

builder.Services.AddHttpClient("StockMarketService", client =>
{
    client.BaseAddress = new Uri("https://stock-serice-bbf0ere3aveye6cr.canadacentral-01.azurewebsites.net/");
});

// Register custom services
builder.Services.AddDistributedMemoryCache(); // or Redis, etc.
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the timeout
});
builder.Services.AddHttpContextAccessor(); // Required for session access
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AuthRepository>(); // Database access for authentication
builder.Services.AddScoped<AuthService>();    // Business logic for authentication
builder.Services.AddSingleton<SessionService>(); // Singleton for session management

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    logger.LogInformation("Swagger is enabled in development mode.");
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseSession(); // Add this here before MapControllers

app.MapControllers();

app.Run();
