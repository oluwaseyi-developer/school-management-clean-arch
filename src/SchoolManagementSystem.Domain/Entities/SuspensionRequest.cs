using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Domain.Entities
{
    public class SuspensionRequest : BaseEntity
    {
        public string Reason { get; set; } = string.Empty;
        public string Evidence { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public SuspensionStatus Status { get; set; } = SuspensionStatus.Pending;
        public DateTime? SuspensionStartDate { get; set; }
        public DateTime? SuspensionEndDate { get; set; }
        public string? AdminComments { get; set; }

        // Foreign Keys
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        public Guid ApprovedById { get; set; }
        public User? ApprovedBy { get; set; }
    }
}
