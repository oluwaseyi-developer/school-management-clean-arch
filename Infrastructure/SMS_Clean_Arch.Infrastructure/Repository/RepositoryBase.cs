using Microsoft.EntityFrameworkCore;
using SMS_Clean_Arch.Domain.Interfaces.Repositories;
using SMS_Clean_Arch.Infrastructure.SMS_Clean_Arch_Context;

namespace SMS_Clean_Arch.Infrastructure.Repository
{
    public class RepositoryBase<Tmodel>: IRepositoryBase<Tmodel> where Tmodel: class
    {
        private readonly SMSCleanArchContext _context;

        public RepositoryBase(SMSCleanArchContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Tmodel entity)
        {
            await _context.Set<Tmodel>().AddAsync(entity);
        }

        public async Task<List<Tmodel>> GetAllAsync()
        {
           return await _context.Set<Tmodel>().AsNoTracking().ToListAsync();
        }

        public async Task<Tmodel?> GetByIdAsync(Guid id)
        {
            return await _context.Set<Tmodel>().FindAsync(id);
        }

        public void Remove(Tmodel entity)
        {
            _context.Set<Tmodel>().Remove(entity);
            _context.SaveChangesAsync(CancellationToken.None);
        }

        public void Update(Tmodel entity)
        {
            _context.Set<Tmodel>().Update(entity);
            _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
