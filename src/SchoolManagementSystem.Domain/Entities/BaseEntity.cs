namespace SchoolManagementSystem.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false; // Soft Delete
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

    }
}
