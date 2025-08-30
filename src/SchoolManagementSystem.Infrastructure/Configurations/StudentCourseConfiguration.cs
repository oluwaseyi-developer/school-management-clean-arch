using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Configurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Grade)
                 .HasPrecision(5, 2); 

            builder.HasOne(sc => sc.Student)
                 .WithMany(s => s.StudentCourses)
                 .HasForeignKey(sc => sc.StudentId)
                 .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sr => sr.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(sc => new
            {
                sc.StudentId,
                sc.CourseId
            })
                .IsUnique();
        }

    }
}
