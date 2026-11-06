using LibraryApi.DTOs;
using LibraryApi.Repositories.Interfaces;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;

namespace LibraryApi.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repository;
    public MemberService(IMemberRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<ResponseMemberDTO>> GetAllAsync()
    {
        var members = await _repository.GetAllAsync();
        return members;
    }
    public async Task<ResponseMemberDTO?> GetByIdAsync(int id)
    {
        var member = await _repository.GetByIdAsync(id);
        if (member == null)
        {
            return null;
        }
        return member;
    }
    public async Task<int> CreateAsync(CreateMemberDTO member)
    {
        int id;
        try
        {
            id = await _repository.CreateAsync(member);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new ArgumentException("Email must be Unique");
        }
        return id;
    }
    public async Task<bool> UpdateAsync(int id, UpdateMemberDTO member)
    {
        bool affectedRows;
        try
        {
            affectedRows = await _repository.UpdateAsync(id, member);
        }
        catch (PostgresException e) when (e.SqlState == "23505")
        {
            throw new ArgumentException("Email must be Unique.");
        }
        return affectedRows;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        bool affectedRows;
        try
        {
            affectedRows = await _repository.DeleteAsync(id);
        }
        catch (PostgresException e) when (e.SqlState == "23503" || e.SqlState == "23001")
        {
            throw new ArgumentException("Member is associated with a loan.");
        }
        return affectedRows;
    }
    public async Task<List<ResponseLoanDTO>> GetLoansByMemberAsync(int id, bool? isActive)
    {
        var loans = await _repository.GetLoansByMemberAsync(id, isActive);
        return loans;
    }
}