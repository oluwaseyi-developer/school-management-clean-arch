using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.Common.Interfaces.Repositories
{
    public interface ISuspensionRequestRepository : IRepository<SuspensionRequest>
    {
        Task<IEnumerable<SuspensionRequest>> GetByStatusAsync(SuspensionStatus status);
        Task<IEnumerable<SuspensionRequest>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<SuspensionRequest>> GetByTeacherIdAsync(Guid teacherId);
        Task<SuspensionRequest> GetWithDetailsAsync(Guid id);
        Task<IEnumerable<SuspensionRequest>> FindSuspensionsAsync(Func<SuspensionRequest, bool> predicate);
    }
}
