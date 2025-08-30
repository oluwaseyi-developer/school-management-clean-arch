namespace SchoolManagementSystem.Domain.Entities
{
    public class Teacher : BaseEntity
    {
        public string TeacherId { get; set; } = string.Empty; // Custom Id Like TCH-YEAR-001
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string SubjectSpecialty { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Foreign Key
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation Property
        public ICollection<SuspensionRequest> SuspensionRequests { get; set; } = new List<SuspensionRequest>();
    }
}
