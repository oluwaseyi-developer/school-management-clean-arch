namespace SMS_Clean_Arch.Domain.Entities
{
    public class Subject
    {
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }


        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
