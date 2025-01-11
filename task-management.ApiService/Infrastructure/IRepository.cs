using System.Linq.Expressions;

namespace task_management.ApiService.Infrastructure;

/// <summary> The is the interface for the Repository </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> where T : class
{
    /// <summary> This method is used to get an entity by its id </summary>
    Task<T> GetAsync(string id, CancellationToken cancellationToken = default);
    /// <summary> This method is used to get all entities based on the predicate </summary>
    /// <remarks>To use this method, you need to pass a predicate such as <code>GetAllAsync(x => x.Id == id)</code></remarks>
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    /// <summary> This method is used to add an entity to the repository </summary>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    /// <summary> This method is used to update an entity in the repository </summary>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    /// <summary> This method is used to delete an entity from the repository </summary>
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}