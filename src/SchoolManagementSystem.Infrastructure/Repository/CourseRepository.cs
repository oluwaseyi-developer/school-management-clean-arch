using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.Infrastructure.Repository
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Course?> GetByCourseCodeAsync(string courseCode)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.CourseCode == courseCode);
        }

        public async Task<Course?> GetCourseWithStudentsAsync(Guid id)
        {
            return await _dbSet
                .Include(c => c.StudentCourses)
                .ThenInclude(sc => sc.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Course>> GetCoursesByCreditsAsync(int minCredits, int maxCredits)
        {
            return await _dbSet
                .Where(c => c.Credits >= minCredits && c.Credits <= maxCredits)
                .ToListAsync();
        }

        public async Task<bool> CourseCodeExistsAsync(string courseCode)
        {
            return await _dbSet.AnyAsync(c => c.CourseCode == courseCode);
        }

        public async Task<IEnumerable<Course>> FindCoursesAsync(Func<Course, bool> predicate)
        {
            var allCourses = await _dbSet
                .Include(c => c.StudentCourses)
                .ToListAsync();

            return allCourses.Where(predicate);
        }
    }
}
