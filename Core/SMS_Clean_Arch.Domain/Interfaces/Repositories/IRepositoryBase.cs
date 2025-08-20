using SMS_Clean_Arch.Domain.Entities;

namespace SMS_Clean_Arch.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<Tmodel>  where Tmodel : class
    {
        Task AddAsync(Tmodel entity);
        Task<List<Tmodel>> GetAllAsync();
        Task<Tmodel?> GetByIdAsync(Guid id);
        void Update(Tmodel entity);
        void Remove(Tmodel entity);
    }

}
