using SchoolManagementSystem.Domain.Entities;
using System.Linq.Expressions;

namespace SchoolManagementSystem.Application.Common.Interfaces.Repositories
{
    public interface IRepository<T>  where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null!);
    }
}
