using SchoolApi.DTO.ClassRoom;

namespace SchoolApi.Repository.Interfaces;

public interface IClassRoomsRepository
{
    Task<List<ResponseClassRoomsDTO>> GetAllAsync();
    Task<ResponseClassRoomsDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateClassRoomsDTO classroom);
    Task<bool> UpdateAsync(int id, UpdateClassRoomsDTO classroom);
    Task<bool> DeleteAsync(int id);
}