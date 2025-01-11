namespace task_management.ApiService.Infrastructure;

/// <summary> The is the interface for the DataContext </summary>
public interface IDataContext {
    /// <summary> Gets the <see cref="IRepository{T}"/> for the specified entity type. </summary>
    IRepository<T> Set<T>() where T : class;
}