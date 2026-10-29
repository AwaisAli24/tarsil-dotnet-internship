using Microsoft.Extensions.Configuration;
using Npgsql;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

string? connectionString = builder.GetConnectionString("DefaultConnection");

using var connection = new NpgsqlConnection(connectionString);

try
{
    await connection.OpenAsync();
}
catch (PostgresException ex)
{
    Console.WriteLine($"Error connecting to the database: {ex.Message}");
    return;
}


using var command = connection.CreateCommand();

command.CommandText = """
    SELECT version();
""";
{

    var result = await command.ExecuteScalarAsync();

    var version = result is null or DBNull ? null : Convert.ToString(result);

    Console.WriteLine(version);
}

command.CommandText = """
    SELECT COUNT(id) FROM books;
""";
{

    var result = await command.ExecuteScalarAsync();
    var count = result is null or DBNull ? null : Convert.ToString(result);
    Console.WriteLine(count);
}
