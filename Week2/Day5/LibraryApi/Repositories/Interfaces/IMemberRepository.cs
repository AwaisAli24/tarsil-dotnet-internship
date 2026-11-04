using LibraryApi.DTOs;
namespace LibraryApi.Repositories.Interfaces;

public interface IMemberRepository
{
    Task<List<ResponseMemberDTO>> GetAllAsync();
    Task<ResponseMemberDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateMemberDTO member);
    Task<bool> UpdateAsync(int id, UpdateMemberDTO member);
    Task<bool> DeleteAsync(int id);
}