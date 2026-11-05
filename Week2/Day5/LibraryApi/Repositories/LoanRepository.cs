using LibraryApi.DTOs;
using LibraryApi.Exceptions;
using LibraryApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;

namespace LibraryApi.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly NpgsqlDataSource _source;
    public LoanRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<int> BorrowAsync(CreateLoanDTO loan)
    {
        int id;
        using var connection = _source.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            using var command = connection.CreateCommand();

            command.CommandText = """
                SELECT EXISTS
                (SELECT * FROM members WHERE id = @member_id)
                and
                EXISTS
                (SELECT * FROM books WHERE id = @book_id)
            """;
            command.Parameters.AddWithValue("@book_id", loan.BookId);
            command.Parameters.AddWithValue("@member_id", loan.MemberId);

            bool isValid = Convert.ToBoolean(await command.ExecuteScalarAsync());

            if (!isValid)
            {
                throw new ArgumentException("Book or member does Not exist.");
            }

            command.CommandText = """
                SELECT COUNT(*) FROM loans
                WHERE member_id=@member_id
                GROUP BY member_id
            """;
            command.Parameters.AddWithValue("@member_id", loan.MemberId);
            int totalLoansOfMember = Convert.ToInt32(await command.ExecuteScalarAsync());

            if (totalLoansOfMember >= 3)
            {
                throw new InvalidOperationException("Member already has 3 loans, cannot take more loans.");
            }

            command.CommandText = """
                SELECT EXISTS
                (SELECT 1 FROM books WHERE id = @id and available_copies>0)
            """;
            command.Parameters.AddWithValue("@id", loan.BookId);
            bool isAvailable = Convert.ToBoolean(await command.ExecuteScalarAsync());

            if (!isAvailable)
            {
                throw new InvalidOperationException("Book is not available.");
            }

            command.CommandText = """
                UPDATE books
                SET available_copies = available_copies - 1
                WHERE id = @book_id;
            """;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@book_id", loan.BookId);
            command.Transaction = transaction;
            int affectedRows = await command.ExecuteNonQueryAsync();

            command.CommandText = """
                INSERT INTO loans(book_id,member_id,borrowed_at,due_date,returned_at)
                VALUES(@book_id,@member_id,@borrowed_at,@due_date,NULL)
                RETURNING id;
            """;
            command.Parameters.AddWithValue("@book_id", loan.BookId);
            command.Parameters.AddWithValue("@member_id", loan.MemberId);
            command.Parameters.AddWithValue("@borrowed_at", DateTime.Now);
            command.Parameters.AddWithValue("@due_date", DateTime.Now.AddDays(loan.days));
            id = Convert.ToInt32(await command.ExecuteScalarAsync());

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        return id;
    }
    public async Task<int> ReturnAsync(int id)
    {
        using var connection = _source.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            using var command = connection.CreateCommand();

            command.CommandText = """
                SELECT EXISTS (
                    SELECT 1 FROM loans 
                    WHERE id = @id
                    );
            """;
            command.Parameters.AddWithValue(id);
            bool exist = Convert.ToBoolean(await command.ExecuteScalarAsync());
            if (!exist)
            {
                throw new ArgumentException("Provided id does not exist in loans.");
            }

            command.CommandText = """
                UPDATE loans
                SET returned_at = @returned_at
                WHERE id = @id and returned_at is NULL
                RETURNING book_id;
            """;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@returned_at", DateTime.Now);
            command.Parameters.AddWithValue("@id", id);
            int bookId = Convert.ToInt32(await command.ExecuteScalarAsync());
            if (bookId == 0)
            {
                throw new InvalidOperationException("The Book is Already returned or the loan does not exist.");
            }
            Console.WriteLine(bookId);
            command.CommandText = """
                UPDATE books
                SET available_books = available_books+1
                WHERE id = @book_id;
            """;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@book_id", bookId);
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        return id;
    }
}