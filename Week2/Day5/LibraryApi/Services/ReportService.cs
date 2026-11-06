using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;
using LibraryApi.DTOs;

public class ReportService : IReportService
{
    public readonly IReportRepository _repository;
    public ReportService(IReportRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseOverdueLoansDTO>> GetOverdueLoansAsync()
    {
        var overdueLoans = await _repository.GetOverdueLoansAsync();
        return overdueLoans;
    }
    public async Task<List<ResponseTopBorrowedDTO>> GetMostBorrowedBooks(int limit)
    {
        var topBooks = await _repository.GetMostBorrowedBooks(limit);
        return topBooks;
    }

}