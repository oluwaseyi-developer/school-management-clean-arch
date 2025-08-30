using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Application.Common.DTOs.Student
{
    public class CreateStudentDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateTime EnrollmentDate { get; set; }
    }
}
