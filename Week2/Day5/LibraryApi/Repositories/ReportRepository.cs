using LibraryApi.DTOs;
using LibraryApi.Repositories.Interfaces;
using Npgsql;

namespace LibraryApi.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly NpgsqlDataSource _source;
    public ReportRepository(NpgsqlDataSource source)
    {
        _source = source;
    }
    public async Task<List<ResponseOverdueLoansDTO>> GetOverdueLoansAsync()
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT t.id,b.title,m.full_name,t.days_overdue FROM 
            (SELECT	id,member_id,
            book_id,
            GREATEST(0,
            NOW()::Date - due_date::date
            ) AS days_overdue
            FROM loans WHERE returned_at IS NULL) as t
            JOIN books b
            ON b.id = t.book_id
            Join members m
            ON m.id = t.member_id
            WHERE days_overdue>0 ;
        """;
        using var reader = await command.ExecuteReaderAsync();

        List<ResponseOverdueLoansDTO> overdueLoans = new List<ResponseOverdueLoansDTO>();
        while (await reader.ReadAsync())
        {
            overdueLoans.Add(new ResponseOverdueLoansDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Title = reader.GetString(reader.GetOrdinal("title")),
                Fullname = reader.GetString(reader.GetOrdinal("full_name")),
                DaysOverdue = reader.GetInt32(reader.GetOrdinal("days_overdue"))
            });
        }
        return overdueLoans;
    }
    public async Task<List<ResponseTopBorrowedDTO>> GetMostBorrowedBooks(int limit)
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT b.id,b.title,a.name,count(l.book_id) FROM books b
            JOIN loans l ON b.id=l.book_id
            JOIN authors a ON b.author_id = a.id
            GROUP BY b.id,a.name
            ORDER BY COUNT(book_id) DESC
            LIMIT @limit;
        """;
        command.Parameters.AddWithValue("@limit", limit);
        using var reader = await command.ExecuteReaderAsync();
        List<ResponseTopBorrowedDTO> books = new List<ResponseTopBorrowedDTO>();
        while (await reader.ReadAsync())
        {
            books.Add(new ResponseTopBorrowedDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Title = reader.GetString(reader.GetOrdinal("title")),
                AuthorName = reader.GetString(reader.GetOrdinal("name")),
                TimesBorrowed = reader.GetInt32(reader.GetOrdinal("count"))
            });
        }
        return books;
    }
    public async Task<List<ResponseAuthorReportDTO>> GetAuthorReports()
    {
        using var command = _source.CreateCommand();
        command.CommandText = """
            SELECT a.name, count(b.id) as total_books,SUM( b.total_copies) as total_copies,SUM(b.total_copies-b.available_copies) as currently_borrowed
            FROM books b
            JOIN authors a
            ON b.author_id = a.id
            GROUP BY a.name;
        """;
        var reader = await command.ExecuteReaderAsync();
        List<ResponseAuthorReportDTO> authors = new List<ResponseAuthorReportDTO>();
        while (await reader.ReadAsync())
        {
            authors.Add(new ResponseAuthorReportDTO
            {
                AuthorName = reader.GetString(reader.GetOrdinal("name")),
                NumberOfBooks = reader.GetInt32(reader.GetOrdinal("total_books")),
                TotalCopies = reader.GetInt32(reader.GetOrdinal("total_copies")),
                CurrentlyBorrowedCopies = reader.GetInt32(reader.GetOrdinal("currently_borrowed"))
            });
        }
        return authors;
    }
}