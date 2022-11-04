using CadastroService.Models;
using CadastroService.Models.Context;
using CadastroService.Profiles;
using CadastroService.Services;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = "server=us-cdbr-east-06.cleardb.net;database=heroku_3a78d4ba4e4af48;user=b5a5f7082e5f8d;password=132cacca";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LeitorDbContext>(option => option.UseMySql(connectionString, ServerVersion.AutoDetect("server=us-cdbr-east-06.cleardb.net;database=heroku_3a78d4ba4e4af48;user=b5a5f7082e5f8d;password=132cacca")));
builder.Services.AddAutoMapper(typeof(LeitorProfile));
builder.Services.AddScoped<CryptoService>();
builder.Services.AddScoped<EmailService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
