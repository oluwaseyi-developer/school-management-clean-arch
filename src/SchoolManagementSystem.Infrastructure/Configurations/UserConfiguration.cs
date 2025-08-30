using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(u => u.Role)
               .IsRequired()
               .HasMaxLength(20);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);
        }
    }
}
