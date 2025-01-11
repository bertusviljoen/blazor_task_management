using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace task_management.ApiService.Infrastructure;

public class CosmosDataContextOptions
{
    public string DatabaseName { get; set; } = string.Empty;
}

public class CosmosDataContext : IDataContext
{
    private readonly Database _database;
    private readonly Dictionary<Type, object> _repositories;

    public CosmosDataContext(IOptions<CosmosDataContextOptions> options,
        CosmosClient cosmosClient)
    {
        var databaseName = options.Value.DatabaseName;
        _database = cosmosClient.GetDatabase(databaseName);
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<T> Set<T>() where T : class
    {
        if (_repositories.TryGetValue(typeof(T), out var repo))
        {
            return (IRepository<T>)repo;
        }

        var repository = new CosmosRepository<T>(_database);
        _repositories.Add(typeof(T), repository);
        return repository;
    }
}