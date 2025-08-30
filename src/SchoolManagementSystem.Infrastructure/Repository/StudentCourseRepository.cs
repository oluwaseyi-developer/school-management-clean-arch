using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.Infrastructure.Repository
{
    public class StudentCourseRepository : BaseRepository<StudentCourse>, IStudentCourseRepository
    {
        public StudentCourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<StudentCourse?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task<IEnumerable<StudentCourse>> GetByStudentIdAsync(Guid studentId)
        {
            return await _dbSet
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentCourse>> GetByCourseIdAsync(Guid courseId)
        {
            return await _dbSet
                .Where(sc => sc.CourseId == courseId)
                .Include(sc => sc.Student)
                .ThenInclude(s => s.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentCourse>> GetByGradeRangeAsync(decimal minGrade, decimal maxGrade)
        {
            return await _dbSet
                .Where(sc => sc.Grade >= minGrade && sc.Grade <= maxGrade)
                .Include(sc => sc.Student)
                .ThenInclude(s => s.User)
                .Include(sc => sc.Course)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentCourse>> FindEnrollmentsAsync(Func<StudentCourse, bool> predicate)
        {
            var allEnrollments = await _dbSet
                .Include(sc => sc.Student)
                .ThenInclude(s => s.User)
                .Include(sc => sc.Course)
                .ToListAsync();

            return allEnrollments.Where(predicate);
        }
    }
}