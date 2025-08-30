using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.Common.Interfaces.Repositories
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        Task<Teacher> GetByTeacherIdAsync(string teacherId);
        Task<Teacher> GetTeacherWithDetailsAsync(Guid id);
        Task<IEnumerable<Teacher>> GetTeachersBySubjectAsync(string subject);
        Task<bool> TeacherIdExistsAsync(string teacherId);
        Task<IEnumerable<Teacher>> FindTeachersAsync(Func<Teacher, bool> predicate);
    }
}
