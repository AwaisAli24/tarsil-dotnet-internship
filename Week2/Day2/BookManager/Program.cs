using Npgsql;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;


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
    return;
}
bool running = true;
while (running)
{
    using var command = connection.CreateCommand();
    Console.WriteLine("1.List Books\n2.Search by title\n3.Add Book\n4.Update Price\n5.Delete Book\n6.Exit");
    bool success = int.TryParse(Console.ReadLine(), out int choice);
    if (!success)
    {
        Console.WriteLine("Invalid choice! Please Enter a valid choice.");
        continue;
    }
    switch (choice)
    {
        case 1:
            {
                command.CommandText = """
                SELECT b.id,title,price,a.name AS Author_Name FROM books b
                JOIN authors a ON author_id=a.id;
            """;

                using var reader = await command.ExecuteReaderAsync();
                Console.WriteLine("Id\tTitle\t\t\t\t\tPrice\t\tAuthor");
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(0);
                    string title = reader.GetString(1);
                    decimal price = reader.GetDecimal(2);
                    string authorName = reader.GetString(3);
                    string space = title.Length < 16 ? "\t" : "";
                    Console.WriteLine($"{id}\t{title}{space}\t\t\t{price}\t\t{authorName}");
                }
            }
            break;
        case 2:
            {
                Console.WriteLine("Enter text to search:");
                string? search = Console.ReadLine();

                command.CommandText = """
                SELECT title FROM books
                WHERE title ILIKE @search;
            """;
                command.Parameters.AddWithValue("@search", "%" + search + "%");
                // NpgsqlDataReader reader;
                try
                {
                    using var reader = await command.ExecuteReaderAsync();
                    Console.WriteLine($"Following books contain \"{search}\" in their title:");
                    while (await reader.ReadAsync())
                    {
                        string title = reader.GetString(0);
                        Console.WriteLine(title);
                    }
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
            break;
        case 3:
            {
                Console.WriteLine("Enter book title:");
                string? title = Console.ReadLine();
                Console.WriteLine("Enter book isbn:");
                string? isbn = Console.ReadLine();
                Console.WriteLine("Enter book price:");
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(isbn))
                {
                    Console.WriteLine("Fields Are required.");
                    continue;
                }
                success = decimal.TryParse(Console.ReadLine(), out decimal price);
                if (!success)
                {
                    Console.WriteLine("Invalid Input! Please Try again");
                    continue;
                }
                Console.WriteLine("Enter total copies:");
                success = int.TryParse(Console.ReadLine(), out int totalCopies);
                if (!success)
                {
                    Console.WriteLine("Invalid Input! Please Try again");
                    continue;
                }
                Console.WriteLine("Enter available copies:");
                success = int.TryParse(Console.ReadLine(), out int availableCopies);
                if (!success)
                {
                    Console.WriteLine("Invalid Input! Please Try again");
                    continue;
                }
                Console.WriteLine("Enter published year:");
                if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime publishedDate))
                {
                    Console.WriteLine("Invalid date format.");
                    continue;
                }
                Console.WriteLine("Enter author's name:");
                string? authorName = Console.ReadLine();
                command.CommandText = """
                    SELECT id FROM authors
                    WHERE name ILIKE @author_name;    
                """;
                command.Parameters.AddWithValue("@author_name", "%" + authorName + "%");
                int authorId;
                try
                {
                    var result = await command.ExecuteScalarAsync();
                    authorId = Convert.ToInt32(result);
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                command.CommandText = """
                    INSERT INTO books(title,isbn,price,total_copies,available_copies,published_year,author_id)
                    VALUES(@title,@isbn,@price,@totalCopies,@availableCopies,@publishedDate,@author_id)
                    RETURNING id;
                """;
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@isbn", isbn);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@totalCopies", totalCopies);
                command.Parameters.AddWithValue("@availableCopies", availableCopies);
                command.Parameters.AddWithValue("@publishedDate", publishedDate);
                command.Parameters.AddWithValue("@author_id", authorId);
                try
                {

                    var result = await command.ExecuteScalarAsync();
                    Console.WriteLine($"Book inserted with id ${Convert.ToInt32(result)}");
                    break;
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
        case 4:
            {
                Console.WriteLine("Enter Book id to update:");
                success = int.TryParse(Console.ReadLine(), out int id);
                if (!success)
                {
                    Console.WriteLine("Invalid Choice");
                    continue;
                }
                command.CommandText = """
                    SELECT price FROM books
                    WHERE id=@id
                """;
                command.Parameters.AddWithValue("@id", id);
                decimal price;
                try
                {

                    var result = await command.ExecuteScalarAsync();
                    price = Convert.ToDecimal(result);

                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                Console.WriteLine($"The current price is: {price}");

                Console.WriteLine("Enter the Updated Price:");
                success = decimal.TryParse(Console.ReadLine(), out decimal newPrice);
                if (!success)
                {
                    Console.Write("Invalid Choice! please try again.");
                    continue;
                }
                command.CommandText = """
                    UPDATE books
                    SET price = @newPrice
                    Where id = @id;
                """;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@newPrice", newPrice);
                int affectedRows = 0;
                try
                {
                    affectedRows = await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Price Updated.");
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
            break;
        case 5:
            {
                Console.Write("Enter Book id to delete:");
                success = int.TryParse(Console.ReadLine(), out int id);
                if (!success)
                {
                    Console.WriteLine("Invalid choice! Please try again");
                }
                command.CommandText = """
                    DELETE FROM books
                    WHERE id = @id
                """;
                command.Parameters.AddWithValue("@id", id);
                int result = 0;
                try
                {
                    result = await command.ExecuteNonQueryAsync();
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                if (result == 1)
                {
                    Console.WriteLine("Deleted Successfully");
                }
            }
            break;
        case 6:
            running = false;
            break;

    }
}

