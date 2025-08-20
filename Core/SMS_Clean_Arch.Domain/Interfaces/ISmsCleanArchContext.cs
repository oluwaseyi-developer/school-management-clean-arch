using Microsoft.EntityFrameworkCore;
using SMS_Clean_Arch.Domain.Entities;


namespace SMS_Clean_Arch.Domain.Interfaces
{
    public interface ISmsCleanArchContext
    {
        DbSet<User> Users { get; }
        DbSet<Department> Departments { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
