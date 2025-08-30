using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> EmailExistAsync(string email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
        Task<User> GetUserWithDetailsAsync(Guid id);
    }
}
