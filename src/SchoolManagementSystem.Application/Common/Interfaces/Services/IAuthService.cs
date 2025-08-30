using SchoolManagementSystem.Application.Common.DTOs.Authentication;

namespace SchoolManagementSystem.Application.Common.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword);

    }
}
