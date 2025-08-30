namespace SchoolManagementSystem.Application.Common.DTOs
{
    // Teacher DTOs
    public class TeacherDto
    {
        public Guid Id { get; set; }
        public string TeacherId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string SubjectSpecialty { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreateTeacherDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string SubjectSpecialty { get; set; } = string.Empty;
    }

    // Course DTOs
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateCourseDto
    {
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
    }

    // SuspensionRequest DTOs
    public class SuspensionRequestDto
    {
        public Guid Id { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Evidence { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? SuspensionStartDate { get; set; }
        public DateTime? SuspensionEndDate { get; set; }
        public string AdminComments { get; set; } = string.Empty;
        public Guid StudentId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid? ApprovedById { get; set; }
    }

    public class CreateSuspensionRequestDto
    {
        public string Reason { get; set; } = string.Empty;
        public string Evidence { get; set; } = string.Empty;
        public Guid StudentId { get; set; }
    }

    // StudentCourse DTOs
    public class StudentCourseDto
    {
        public Guid Id { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public decimal? Grade { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }

    public class EnrollStudentDto
    {
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
    }
}
