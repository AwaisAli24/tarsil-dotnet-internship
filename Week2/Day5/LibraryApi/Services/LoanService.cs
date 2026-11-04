using LibraryApi.DTOs;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;
using Npgsql;

public class LoanService : ILoanService
{
    public readonly ILoanRepository _repository;
    public LoanService(ILoanRepository repository)
    {
        _repository = repository;
    }
    public async Task<int> BorrowAsync(CreateLoanDTO loan)
    {
        int id;
        try
        {
            id = await _repository.BorrowAsync(loan);
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e);
            return -1;
        }
        return id;
    }

}