using LibraryApi.DTOs;
using LibraryApi.Repositories.Interfaces;
using Npgsql;
namespace LibraryApi.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;
    private readonly ILogger<AuthorService> _logger;
    public AuthorService(IAuthorRepository repository, ILogger<AuthorService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<List<ResponseAuthorDTO>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
    public async Task<ResponseAuthorDTO?> GetByIdAsync(int id)
    {
        var author = await _repository.GetByIdAsync(id);
        if (author == null)
        {
            return null;
        }
        return author;
    }
    public async Task<ResponseAuthorDTO> CreateAsync(CreateAuthorDTO author)
    {
        ResponseAuthorDTO toAdd;
        try
        {
            toAdd = await _repository.CreateAsync(author);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new ArgumentException("Author already Exist");
        }
        _logger.LogInformation($"Author with id {toAdd.Id} created successfully.");
        return toAdd;
    }
    public async Task<ResponseAuthorDTO?> UpdateAsync(int id, UpdateAuthorDTO author)
    {
        ResponseAuthorDTO? toUpdate;
        try
        {
            toUpdate = await _repository.UpdateAsync(id, author);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new ArgumentException("Author already Exist");
        }
        _logger.LogInformation($"Author with id {toUpdate?.Id} updated successfully.");
        return toUpdate;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        bool toDelete;
        try
        {
            toDelete = await _repository.DeleteAsync(id);
        }
        catch (PostgresException e) when (e.SqlState == "23503" || e.SqlState == "23001")
        {
            throw new ArgumentException("Author is associated with a book and cannot be deleted.");
        }
        if (toDelete)
        {
            _logger.LogInformation($"Author with id {id} deleted successfully.");
        }
        return toDelete;
    }
}