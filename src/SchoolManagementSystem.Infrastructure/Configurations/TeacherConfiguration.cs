using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TeacherId)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(t => t.TeacherId)
                .IsUnique();

            builder.Property(t => t.SubjectSpecialty)
                .HasMaxLength(100);

            builder.HasOne(t => t.User)
                 .WithMany(u => u.Teachers)
                 .HasForeignKey(t => t.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
