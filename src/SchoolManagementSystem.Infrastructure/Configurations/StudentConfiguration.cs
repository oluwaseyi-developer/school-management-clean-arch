using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StudentId)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(s => s.StudentId)
                .IsUnique();

            builder.Property(s => s.Address)
               .HasMaxLength(200);

            builder.Property(s => s.StudentId)
               .HasMaxLength(20);

            builder.HasOne(s => s.User)
                .WithMany(u => u.Students)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
