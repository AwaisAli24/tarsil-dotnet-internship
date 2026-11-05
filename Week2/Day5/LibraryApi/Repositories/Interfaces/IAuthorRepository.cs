using LibraryApi.DTOs;
namespace LibraryApi.Repositories.Interfaces;

public interface IAuthorRepository
{
    Task<List<ResponseAuthorDTO>> GetAllAsync();
    Task<ResponseAuthorDTOWithBooks?> GetByIdAsync(int id);
    Task<ResponseAuthorDTO> CreateAsync(CreateAuthorDTO author);
    Task<ResponseAuthorDTO?> UpdateAsync(int id, UpdateAuthorDTO author);
    Task<bool> DeleteAsync(int id);
}