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
                if (price < 0)
                {
                    Console.WriteLine("Price cannot be negative.");
                    continue;
                }
                Console.WriteLine("Enter total copies:");
                success = int.TryParse(Console.ReadLine(), out int totalCopies);
                if (!success)
                {
                    Console.WriteLine("Invalid Input! Please Try again");
                    continue;
                }
                if (totalCopies < 0)
                {
                    Console.WriteLine("Total copies cannot be negative.");
                    continue;
                }
                Console.WriteLine("Enter available copies:");
                success = int.TryParse(Console.ReadLine(), out int availableCopies);
                if (!success)
                {
                    Console.WriteLine("Invalid Input! Please Try again");
                    continue;
                }
                if (availableCopies < 0)
                {
                    Console.WriteLine("Available copies cannot be negative.");
                    continue;
                }
                if (availableCopies > totalCopies)
                {
                    Console.WriteLine("Available copies cannot be greater than total copies.");
                    continue;
                }
                Console.WriteLine("Enter published year:");
                if (int.TryParse(Console.ReadLine(), out int publishedYear))
                {
                    if (publishedYear < 0 || publishedYear > DateTime.Now.Year)
                    {
                        Console.WriteLine("Published year must be a valid year.");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                    continue;
                }
                Console.WriteLine("Enter author's name:");
                string? authorName = Console.ReadLine();
                if (authorName == null)
                {
                    Console.WriteLine("Author name is required.");
                    continue;
                }
                command.CommandText = """
                    SELECT id FROM authors
                    WHERE name ILIKE @author_name;    
                """;
                command.Parameters.AddWithValue("@author_name", authorName);
                int authorId;
                try
                {
                    var result = await command.ExecuteScalarAsync();
                    if (result is null or DBNull)
                    {
                        Console.WriteLine("Author not found.");
                        continue;
                    }
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
                command.Parameters.AddWithValue("@publishedDate", publishedYear);
                command.Parameters.AddWithValue("@author_id", authorId);
                try
                {

                    var result = await command.ExecuteScalarAsync();
                    Console.WriteLine($"Book inserted with id {Convert.ToInt32(result)}");
                    break;
                }
                catch (NpgsqlException e)
                {
                    if (e.SqlState == "23505")
                    {
                        Console.WriteLine("A book with this ISBN already exists.");
                    }
                    else if (e.SqlState == "23503")
                    {
                        Console.WriteLine("The specified author does not exist.");
                    }
                    else if (e.SqlState == "22001")
                    {
                        Console.WriteLine("One of the input values is too long for the database column.");
                    }
                    else
                    {
                        Console.WriteLine($"Error inserting book: {e.Message}");
                    }
                    continue;
                }
                catch (Exception e)
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
                    if (result == null || result is DBNull)
                    {
                        Console.WriteLine("No book found with the given id.");
                        continue;
                    }
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
                    if (affectedRows == 0)
                    {
                        Console.WriteLine("No book found with the given id.");
                        continue;
                    }
                    Console.WriteLine($"Price updated to {newPrice} for book with id {id}.");
                    Console.WriteLine($"Affected rows: {affectedRows}");
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
                    continue;
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
                else
                {
                    Console.WriteLine("No book found with the given id.");
                }
            }
            break;
        case 6:
            running = false;
            break;
        default:
            Console.WriteLine("Invalid choice! Please try again.");
            continue;
    }
}

