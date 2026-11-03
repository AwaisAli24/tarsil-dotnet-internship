using LibraryApi.Data;
using LibraryApi.Repositories;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services;
using LibraryApi.Services.Interfaces;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration.AddUserSecrets<Program>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    throw new Exception("Connection String is not set");
}

builder.Services.AddSingleton(NpgsqlDataSource.Create(connectionString));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddSingleton<DatabaseMigrator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


var migrator = app.Services.GetRequiredService<DatabaseMigrator>();
await migrator.RunAsync();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
