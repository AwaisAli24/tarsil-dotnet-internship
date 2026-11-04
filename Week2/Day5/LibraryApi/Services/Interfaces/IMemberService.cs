using LibraryApi.DTOs;

namespace LibraryApi.Services.Interfaces;

public interface IMemberService
{
    Task<List<ResponseMemberDTO>> GetAllAsync();
    Task<ResponseMemberDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateMemberDTO member);
    Task<bool> UpdateAsync(int id, UpdateMemberDTO member);
    Task<bool> DeleteAsync(int id);
}