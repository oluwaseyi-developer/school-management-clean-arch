namespace SchoolManagementSystem.Application.Common.DTOs.Student
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public bool IsSuspended { get; set; }
        public DateTime? SuspensionEndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
