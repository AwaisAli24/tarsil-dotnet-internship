using SchoolApi.DTO.Students;

public interface IStudentsRepository
{
    Task<List<ResponseStudentsDTO>> GetAllAsync();
    Task<ResponseStudentsDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateStudentsDTO student);
    Task<bool> UpdateAsync(int id, UpdateStudentsDTO student);
    Task<bool> DeleteAsync(int id);
}