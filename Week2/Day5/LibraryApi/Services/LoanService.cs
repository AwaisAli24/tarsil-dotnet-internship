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
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException(e.Message);
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException(e.Message);
        }
        return id;
    }
    public async Task<int> ReturnAsync(int id)
    {
        try
        {
            await _repository.ReturnAsync(id);
        }
        catch
        {
            throw;
        }
        return id;
    }

}