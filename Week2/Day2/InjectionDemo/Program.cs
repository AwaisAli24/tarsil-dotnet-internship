using Npgsql;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

string? connectionString = builder.GetConnectionString("DefaultConnection");

using var connection = new Npgsql.NpgsqlConnection(connectionString);

try
{
    await connection.OpenAsync();
}
catch (NpgsqlException e)
{
    Console.WriteLine(e.Message);
}

var command = connection.CreateCommand();

async Task FindMemberUnsafe(string email)
{
    command.CommandText = $"""
        SELECT * FROM emails
        WHERE email = '{email}';
    """;
    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        int id = reader.GetInt32(0);
        string data = reader.GetString(1);
        Console.WriteLine($"{id}\t\t{data}");
    }
}
async Task FindMemberSafe(string email)
{
    command.CommandText = """
        SELECT * FROM emails
        Where email = @email;
    """;
    command.Parameters.AddWithValue("@email", email);
    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
        int id = reader.GetInt32(0);
        string data = reader.GetString(1);
        Console.WriteLine($"{id}\t\t{data}");
    }
}

await FindMemberUnsafe("' OR '1'='1");
await FindMemberSafe("awais@mail.com");