using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS_Clean_Arch.Domain.Entities;

namespace SMS_Clean_Arch.Infrastructure.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Description)
                   .HasMaxLength(250);

            builder.Property(d => d.CreatedAt)
                   .IsRequired();

            builder.Property(d => d.UpdatedAt)
                   .IsRequired();

            builder.HasMany(d => d.Subjects)
                   .WithOne(s => s.Department)
                   .HasForeignKey(s => s.DepartmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
