using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.Infrastructure.Repository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository                                                       
    {
        public StudentRepository(ApplicationDbContext context) : base(context) {}

        public async Task<IEnumerable<Student>> FindStudentsAsync(Func<Student, bool> predicate)
        {
            var allStudents = await _dbSet
                 .Include(s => s.User)
                 .Include(s => s.StudentCourses)
                 .ToListAsync();

            return allStudents.Where(predicate);
        }

        public async Task<Student?> GetByStudentIdAsync(string studentId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.StudentId == studentId);
        }

        public async Task<IEnumerable<Student>> GetStudentsByCourseAsync(Guid courseId)
        {
            return await _dbSet.Include(s => s.StudentCourses)
                .Where(s => s.StudentCourses.Any(sc => sc.CourseId == courseId))
                .ToListAsync();
        }

        public async Task<Student?> GetStudentWithDetailsAsync(Guid id)
        {
            return await _dbSet.Include(s => s.User)
                .Include(s => s.SuspensionRequests)
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetSuspendedStudentsAsync()
        {
            return await _dbSet.Where(s => s.IsSuspended).ToListAsync();
        }

        public async Task<bool> StudentIdExistsAsync(string studentId)
        {
            return await _dbSet.AnyAsync(s => s.StudentId == studentId);
        }
    }
}
