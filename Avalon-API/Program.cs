using Microsoft.EntityFrameworkCore;
using Avalon_API.Models;
using Microsoft.Extensions.Logging;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
string logFilePath = "./logs/mylog.txt"; // Spécifiez le chemin de fichier souhaité
builder.Logging.AddProvider(new FileLoggerProvider(logFilePath));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<PhotoContext>(opt =>
    opt.UseInMemoryDatabase("Photo"));
builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseInMemoryDatabase("User"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Custom Metrics to count requests for each endpoint and the method
var counter = Metrics.CreateCounter("peopleapi_path_counter", "Counts requests to the People API endpoints", new CounterConfiguration
{
LabelNames = new[] { "method", "endpoint" }
});

app.UseMetricServer();
app.UseHttpMetrics();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

List<string> allowedOrigins = new List<string>
{
    "http://localhost:5173",
    "http://localhost:8080",
    "http://localhost:3000",
    "http://127.0.0.1:8080"
};

app.UseCors(builder => builder.SetIsOriginAllowed(origin => allowedOrigins.Contains(origin))
    .AllowAnyHeader()
    .AllowAnyMethod());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
