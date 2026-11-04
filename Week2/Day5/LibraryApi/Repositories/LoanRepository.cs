using LibraryApi.DTOs;
using LibraryApi.Repositories.Interfaces;
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
        using var transaction = await connection.BeginTransactionAsync();
        await connection.OpenAsync();
        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = """
                UPDATE books
                SET available_copies = available_copies - 1
                WHERE id = @book_id and available_copies>0;
            """;
            command.Transaction = transaction;
            await command.ExecuteNonQueryAsync();

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
            throw new ArgumentException("Book is not available");
        }
        return id;
    }
}