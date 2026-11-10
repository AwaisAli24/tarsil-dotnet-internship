using Npgsql;
using SchoolApi.Data;
using SchoolApi.Repository.Interfaces;
using SchoolApi.Services;
using SchoolApi.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration.AddUserSecrets<Program>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    throw new ArgumentException("Connection string is not set");
}

builder.Services.AddSingleton(NpgsqlDataSource.Create(connectionString));

builder.Services.AddScoped<IClassRoomsRepository, ClassRoomsRepository>();
builder.Services.AddScoped<IClassRoomsService, ClassRoomsService>();

builder.Services.AddSingleton<DataBaseMigrator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var migrator = app.Services.GetRequiredService<DataBaseMigrator>();
await migrator.RunAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
