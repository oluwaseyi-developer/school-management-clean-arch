using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.Common.Interfaces.Repositories
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course> GetByCourseCodeAsync(string courseCode);
        Task<Course> GetCourseWithStudentsAsync(Guid id);
        Task<IEnumerable<Course>> GetCoursesByCreditsAsync(int minCredits, int maxCredits);
        Task<bool> CourseCodeExistsAsync(string courseCode);
        Task<IEnumerable<Course>> FindCoursesAsync(Func<Course, bool> predicate);
    }
}
