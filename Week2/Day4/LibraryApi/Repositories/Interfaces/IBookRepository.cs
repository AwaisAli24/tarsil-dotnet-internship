using LibraryApi.DTOs;
namespace LibraryApi.Repositories.Interfaces;

public interface IBookRepository
{
    Task<List<ResponseBookDTO>> GetAllAsync(int page, int pageSize, string? title = null, int? authorId = null, decimal? minPrice = null, decimal? maxPrice = null);
    Task<ResponseBookDTO?> GetByIdAsync(int id);
    Task<ResponseBookDTO> CreateAsync(CreateBookDTO book);
    Task<ResponseBookDTO?> UpdateAsync(int id, UpdateBookDTO book);
    Task<bool> DeleteAsync(int id);
}