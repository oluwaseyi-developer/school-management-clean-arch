using SchoolManagementSystem.Application.Common.DTOs.User;

namespace SchoolManagementSystem.Application.Common.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<bool> UpdateUserAsync(Guid id, UserDto userDto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> RestoreUserAsync(Guid id);
    }
}
