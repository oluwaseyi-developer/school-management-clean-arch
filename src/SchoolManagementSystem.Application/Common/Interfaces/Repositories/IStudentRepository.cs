using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.Common.Interfaces.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student> GetByStudentIdAsync(string studentId);
        Task<Student> GetStudentWithDetailsAsync(Guid id);
        Task<IEnumerable<Student>> GetSuspendedStudentsAsync();
        Task<IEnumerable<Student>> GetStudentsByCourseAsync(Guid courseId);
        Task<bool> StudentIdExistsAsync(string studentId);
        Task<IEnumerable<Student>> FindStudentsAsync(Func<Student, bool> predicate);  
    }
}
