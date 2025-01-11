using System.Linq.Expressions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace task_management.ApiService.Infrastructure;

public class CosmosRepository<T> : IRepository<T> where T : class
{
    private readonly Container _container;

    public CosmosRepository(Database database)
    {
        var containerName = typeof(T).Name;
        _container = database.GetContainer(containerName);
    }

    public async Task<T> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id), cancellationToken: cancellationToken);
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var query = _container.GetItemLinqQueryable<T>()
            .Where(predicate)
            .AsQueryable();

        var iterator = query.ToFeedIterator();
        var results = new List<T>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync(cancellationToken);
            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _container.CreateItemAsync(entity, new PartitionKey(GetPartitionKey(entity)),
            null, cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _container.UpsertItemAsync(entity, new PartitionKey(GetPartitionKey(entity)), cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _container.DeleteItemAsync<T>(id, new PartitionKey(id), cancellationToken: cancellationToken);
    }

    private string GetPartitionKey(T entity)
    {
        // Implement logic to extract the partition key from the entity
        // For example, if entities have an 'Id' property:
        var property = typeof(T).GetProperty("Id");
        return property?.GetValue(entity)?.ToString();
    }
}