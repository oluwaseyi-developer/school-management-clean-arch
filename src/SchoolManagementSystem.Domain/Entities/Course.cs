namespace SchoolManagementSystem.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string CourseDescription { get; set; } = string.Empty;
        public int Credits { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Property
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
