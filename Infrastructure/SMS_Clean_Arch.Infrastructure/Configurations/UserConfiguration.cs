using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS_Clean_Arch.Domain.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired().HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired().HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired().HasMaxLength(100);

        builder.Property(u => u.PhoneNumber)
            .IsRequired().HasMaxLength(50);

        builder.HasOne(u => u.Department)
               .WithMany()
               .HasForeignKey(u => u.DepartmentId);
    }
}
