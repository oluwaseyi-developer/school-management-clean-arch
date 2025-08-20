using Microsoft.EntityFrameworkCore;
using SMS_Clean_Arch.Domain.Entities;
using SMS_Clean_Arch.Domain.Interfaces;

namespace SMS_Clean_Arch.Infrastructure.SMS_Clean_Arch_Context
{
    public class SMSCleanArchContext : DbContext, ISmsCleanArchContext
    {
        // Database Creation
        public SMSCleanArchContext(DbContextOptions<SMSCleanArchContext> options) :base(options)  { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This helps to apply configuration on each entities
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SMSCleanArchContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        // Setting Tables In The Database
        public DbSet<User> Users => Set<User>();

        public DbSet<Department> Departments => Set<Department>();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
