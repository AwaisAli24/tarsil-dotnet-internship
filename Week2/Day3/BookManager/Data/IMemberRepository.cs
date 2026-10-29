using BookManager.Models;
namespace BookManager.Data;

public interface IMemberRepository
{
    Task<List<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<int> CreateAsync(Member memebr);
    Task<bool> UpdateAsync(int id, Member member);
    Task<bool> DeleteAsync(int id);
}