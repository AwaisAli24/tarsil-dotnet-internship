using LibraryApi.DTOs;
namespace LibraryApi.Services.Interfaces;

public interface ILoanService
{
    Task<int> BorrowAsync(CreateLoanDTO loan);
}