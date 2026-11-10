using SchoolApi.DTO.ClassRoom;

namespace SchoolApi.Services.Interface;

public interface IClassRoomsService
{
    Task<List<ResponseClassRoomsDTO>> GetAllAsync();
    Task<ResponseClassRoomsDTO?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateClassRoomsDTO course);
    Task<bool> UpdateAsync(int id, UpdateClassRoomsDTO classroom);
    Task<bool> DeleteAsync(int id);
}