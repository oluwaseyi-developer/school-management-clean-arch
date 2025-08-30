namespace SchoolManagementSystem.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using SchoolManagementSystem.Application.Common.Interfaces;
    using SchoolManagementSystem.Infrastructure.Data;

    public class UniqueIdGenerator : IUniqueIdGenerator
    {
        private readonly ApplicationDbContext _context;

        public UniqueIdGenerator(ApplicationDbContext context)
        {
            _context = context;
        }

        // I am Generating Unique Id for my StudentId, TeacherId, CourseId
        private async Task<string> GenerateIdAsync(string prefix, IQueryable<string> existingIds)
        {
            int year = DateTime.UtcNow.Year;

            // Find the last code for this year
            var lastId = await existingIds
                .Where(id => id.StartsWith($"{prefix}-{year}"))
                .OrderByDescending(id => id)   // this will sort the string
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastId != null)
            {
                // Example: STU-2029-0005  this split and get "0005"
                var parts = lastId.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
                    nextNumber = lastNumber + 1;
            }

            return $"{prefix}-{year}-{nextNumber:D4}";
        }

        public Task<string> GenerateStudentIdAsync()
        {
            return GenerateIdAsync("STU", _context.Students.Select(s => s.StudentId));
        }

        public Task<string> GenerateTeacherIdAsync()
        {
            return GenerateIdAsync("TCH", _context.Teachers.Select(t => t.TeacherId));
        }
    }

}
