namespace SchoolManagementSystem.Domain.Entities
{
    public class StudentCourse : BaseEntity
    {
        public DateTime EnrollmentDate { get; set; }
        public decimal? Grade { get; set; }


        // Foreign Keys
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
