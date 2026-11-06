using LibraryApi.DTOs;

namespace LibraryApi.Repositories.Interfaces;

public interface IReportRepository
{
    Task<List<ResponseOverdueLoansDTO>> GetOverdueLoansAsync();
    Task<List<ResponseTopBorrowedDTO>> GetMostBorrowedBooks(int limit);
    // Task<List<ResponseAuthorDTOWithBooks>> GetAuthorReports();
}