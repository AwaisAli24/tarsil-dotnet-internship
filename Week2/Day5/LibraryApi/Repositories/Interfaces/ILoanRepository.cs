using LibraryApi.DTOs;

namespace LibraryApi.Repositories.Interfaces;

public interface ILoanRepository
{
    Task<int> BorrowAsync(CreateLoanDTO loan);
}