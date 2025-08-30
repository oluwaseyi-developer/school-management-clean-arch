using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.Infrastructure.Repository
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Teacher>> FindTeachersAsync(Func<Teacher, bool> predicate)
        {
            var allTeachers = await _dbSet.Include(t => t.User)
                .Include(t => t.SuspensionRequests)
                .ToListAsync();

            return allTeachers.Where(predicate);
        }

        public async Task<Teacher?> GetByTeacherIdAsync(string teacherId)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.TeacherId == teacherId);
        }

        public async Task<IEnumerable<Teacher>> GetTeachersBySubjectAsync(string subject)
        {
            return await _dbSet.Where(t => t.SubjectSpecialty.Contains(subject)).ToListAsync();

        }

        public async Task<Teacher?> GetTeacherWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.SuspensionRequests)
                .ThenInclude(sr => sr.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> TeacherIdExistsAsync(string teacherId)
        {
            return await _dbSet.AnyAsync(t => t.TeacherId == teacherId);
        }
    }
}
