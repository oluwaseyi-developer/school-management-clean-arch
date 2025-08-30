using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.Common.Interfaces.Repositories
{
    public interface IStudentCourseRepository : IRepository<StudentCourse>
    {
        Task<StudentCourse> GetByStudentAndCourseAsync(Guid studentId, Guid courseId);
        Task<IEnumerable<StudentCourse>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<StudentCourse>> GetByCourseIdAsync(Guid courseId);
        Task<IEnumerable<StudentCourse>> GetByGradeRangeAsync(decimal minGrade, decimal maxGrade);
        Task<IEnumerable<StudentCourse>> FindEnrollmentsAsync(Func<StudentCourse, bool> predicate);
    }
}
