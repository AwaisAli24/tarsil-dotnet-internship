using LibraryApi.DTOs;
namespace LibraryApi.Services;

public interface IAuthorService
{
    Task<List<ResponseAuthorDTO>> GetAllAsync();
    Task<ResponseAuthorDTOWithBooks?> GetByIdAsync(int id);
    Task<ResponseAuthorDTO> CreateAsync(CreateAuthorDTO author);
    Task<ResponseAuthorDTO?> UpdateAsync(int id, UpdateAuthorDTO author);
    Task<bool> DeleteAsync(int id);
}