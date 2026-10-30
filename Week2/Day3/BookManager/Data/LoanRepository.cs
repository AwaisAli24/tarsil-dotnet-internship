using System.Data.Common;
using BookManager.Data;
using Npgsql;

namespace BookManager.Data;

public class LoanRepository : ILoanRepository
{
    private readonly NpgsqlDataSource _dataSource;
    public LoanRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    public async Task BorrowAsync(int bookId, int memberId, int days)
    {
        using var connection = _dataSource.CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {


            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = """
            INSERT INTO loans(book_id,member_id,borrowed_at,due_date,returned_at)
            VALUES(@book_id,@member_id,@borrowed_at,@due_date,null)
            RETURNING id;
        """;
            command.Parameters.AddWithValue("@book_id", bookId);
            command.Parameters.AddWithValue("@member_id", memberId);
            command.Parameters.AddWithValue("@borrowed_at", DateTime.UtcNow);
            command.Parameters.AddWithValue("@due_date", DateTime.UtcNow.AddDays(days));

            var loanId = await command.ExecuteScalarAsync();

            // throw new Exception("Exception occured");
            command.CommandText = """
            UPDATE books
            SET available_copies = available_copies - 1
            WHERE id = @id;
        """;
            command.Parameters.AddWithValue("@id", bookId);

            await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            Console.WriteLine($"Loan added with the id of ${loanId}");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
    public async Task ReturnLoan(int loanId)
    {
        using var isAlreadyReturendCommand = _dataSource.CreateCommand("""
            SELECT EXISTS(
                SELECT * FROM loans
                WHERE id = @loanId and returned_at is NOT NULL
            );
        """);
        isAlreadyReturendCommand.Parameters.AddWithValue("@loanId", loanId);
        bool isAlreadyReturend = Convert.ToBoolean(await isAlreadyReturendCommand.ExecuteScalarAsync());
        if (isAlreadyReturend)
        {
            Console.WriteLine("This loan is already returned");
            return;
        }

        using var connection = await _dataSource.OpenConnectionAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;

            command.CommandText = """
                UPDATE loans
                SET returned_at = @returned_at
                WHERE id = @id
                RETURNING book_id;
            """;
            command.Parameters.AddWithValue("@returned_at", DateTime.UtcNow);
            command.Parameters.AddWithValue("@id", loanId);

            int bookId = Convert.ToInt32(await command.ExecuteScalarAsync());

            command.Parameters.Clear();

            command.CommandText = """
                UPDATE books
                SET available_copies = available_copies+1
                WHERE id = @id;
            """;
            command.Parameters.AddWithValue("@id", bookId);
            await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();
            Console.WriteLine("Loan returned successfully.");
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}