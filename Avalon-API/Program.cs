using Microsoft.EntityFrameworkCore;
using Avalon_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<PhotoContext>(opt =>
    opt.UseInMemoryDatabase("Photo"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
    "http://localhost:3000"
};

app.UseCors(builder => builder.SetIsOriginAllowed(origin => allowedOrigins.Contains(origin))
    .AllowAnyHeader()
    .AllowAnyMethod());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
