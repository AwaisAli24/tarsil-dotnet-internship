using Microsoft.Extensions.Hosting;
using Npgsql;
using BookManager.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    Console.WriteLine("ConnectionString is not set.");
    return;
}
builder.Services.AddSingleton(NpgsqlDataSource.Create(connectionString));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();


using var host = builder.Build();

await host.StartAsync();

using (var scope = host.Services.CreateScope())
{
    var repository = scope.ServiceProvider
        .GetRequiredService<IBookRepository>();

    var books = await repository.GetAllAsync();

    foreach (var book in books)
    {
        Console.WriteLine(book.Title);
    }
}

await host.StopAsync();
