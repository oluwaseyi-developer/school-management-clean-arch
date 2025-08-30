using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Infrastructure.Configurations
{
    public class SuspensionRequestConfiguration : IEntityTypeConfiguration<SuspensionRequest>
    {
        public void Configure(EntityTypeBuilder<SuspensionRequest> builder)
        {
            builder.HasKey(sr => sr.Id);

            builder.Property(sr => sr.Reason)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(sr => sr.Status)
                .HasConversion<string>()
                .HasDefaultValue(SuspensionStatus.Pending);

            builder.HasOne(sr => sr.Student)
                .WithMany(s => s.SuspensionRequests)
                .HasForeignKey(sr => sr.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sr => sr.Teacher)
                .WithMany(t => t.SuspensionRequests)
                .HasForeignKey(sr => sr.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sr => sr.ApprovedBy)
                .WithMany()
                .HasForeignKey(sr => sr.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
