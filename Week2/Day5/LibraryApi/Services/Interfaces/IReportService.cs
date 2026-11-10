using LibraryApi.DTOs;
namespace LibraryApi.Services.Interfaces;

public interface IReportService
{
    Task<List<ResponseOverdueLoansDTO>> GetOverdueLoansAsync();
    Task<List<ResponseTopBorrowedDTO>> GetMostBorrowedBooks(int limit);
    Task<List<ResponseAuthorReportDTO>> GetAuthorReports();
}