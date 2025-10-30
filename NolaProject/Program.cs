using Microsoft.EntityFrameworkCore;
using NolaProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Pega a Connection String do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registra o NolaDbContext
builder.Services.AddDbContext<NolaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Define um nome para nossa política de CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Adiciona o serviço de CORS
builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy => {
                          // Permite que o seu front-end (React) acesse a API
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
