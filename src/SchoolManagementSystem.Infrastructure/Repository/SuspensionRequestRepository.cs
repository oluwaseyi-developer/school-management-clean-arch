using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common.Interfaces.Repositories;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Enums;
using SchoolManagementSystem.Infrastructure.Data;

namespace SchoolManagementSystem.Infrastructure.Repository
{
    public class SuspensionRequestRepository : BaseRepository<SuspensionRequest>, ISuspensionRequestRepository
    {
        public SuspensionRequestRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<SuspensionRequest>> GetByStatusAsync(SuspensionStatus status)
        {
            return await _dbSet
                .Where(sr => sr.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<SuspensionRequest>> GetByStudentIdAsync(Guid studentId)
        {
            return await _dbSet
                .Where(sr => sr.StudentId == studentId)
                .Include(sr => sr.Teacher)
                .ThenInclude(t => t.User)
                .Include(sr => sr.ApprovedBy)
                .ToListAsync();
        }

        public async Task<IEnumerable<SuspensionRequest>> GetByTeacherIdAsync(Guid teacherId)
        {
            return await _dbSet
                .Where(sr => sr.TeacherId == teacherId)
                .Include(sr => sr.Student)
                .ThenInclude(s => s.User)
                .Include(sr => sr.ApprovedBy)
                .ToListAsync();
        }

        public async Task<SuspensionRequest?> GetWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(sr => sr.Student)
                .ThenInclude(s => s.User)
                .Include(sr => sr.Teacher)
                .ThenInclude(t => t.User)
                .Include(sr => sr.ApprovedBy)
                .FirstOrDefaultAsync(sr => sr.Id == id);
        }

        public async Task<IEnumerable<SuspensionRequest>> FindSuspensionsAsync(Func<SuspensionRequest, bool> predicate)
        {
            var allSuspensions = await _dbSet
                .Include(sr => sr.Student)
                .ThenInclude(s => s.User)
                .Include(sr => sr.Teacher)
                .ThenInclude(t => t.User)
                .ToListAsync();

            return allSuspensions.Where(predicate);
        }
    }
}
