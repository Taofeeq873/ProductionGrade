using System.Linq.Expressions;

namespace Application.Contracts.Repositories;

public interface ICrud<T> where T : notnull
{
    Task<T?> ReadAsync(Guid id);
    Task<T> UpdateAsync(T item);
    Task DeleteAsync(T item);
    Task<T> CreateAsync(T item);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool include = false);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, bool include = false);

}