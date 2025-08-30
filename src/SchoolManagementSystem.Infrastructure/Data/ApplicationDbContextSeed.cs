using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Application.Common.Interfaces.Securities;
using SchoolManagementSystem.Domain.Entities;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Infrastructure.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            if (!await context.Users.AnyAsync())
            {
                var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();

                var adminUser = new User
                {
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@School.com",
                    Role = UserRole.Admin.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                // Generate Password Hash and Salt
                passwordHasher.CreatePassword("Admin@123", out var hash, out var salt);
                adminUser.PasswordHash = hash;
                adminUser.PasswordSalt = salt;

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}
