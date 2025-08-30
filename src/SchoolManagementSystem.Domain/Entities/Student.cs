namespace SchoolManagementSystem.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string StudentId { get; set; } = string.Empty; // Custom Id like STU-YEAR-001
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; } = true;


        public bool IsSuspended { get; set; } = false;
        public DateTime SuspensionEndDate { get; set; }
        public string SuspensionReason { get; set; } = string.Empty;

        // Foreign Key
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation Properties
        public ICollection<SuspensionRequest> SuspensionRequests { get; set; } = new List<SuspensionRequest>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

    }
}
