using Microsoft.Azure.Cosmos;
using task_management.ApiService.Models;
using task_management.Shared.Models;

namespace task_management.ApiService.Infrastructure;

internal sealed class DatabaseInitializer(
    CosmosClient cosmosClient,
    IConfiguration configuration,
    ILogger<DatabaseInitializer> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Starting database initialization.");

            await EnsureDatabaseExists();
            await InitializeDatabase();
            await SeedInitialData();

            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
        }
    }

    private async Task EnsureDatabaseExists()
    {
        var databaseName = configuration["CosmosDb:DatabaseName"] ?? "task-management-db";

        var databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
        logger.LogInformation("Database {DatabaseName} ready", databaseResponse.Database.Id);
    }

    private async Task InitializeDatabase()
    {
        var databaseName = configuration["CosmosDb:DatabaseName"] ?? "task-management-db";
        var containerName = configuration["CosmosDb:ContainerName"] ?? nameof(Category);

        var database = cosmosClient.GetDatabase(databaseName);

        // Create container with partition key if it doesn't exist
        await database.CreateContainerIfNotExistsAsync(new ContainerProperties
        {
            Id = containerName,
            PartitionKeyPath = "/id"
        });

        logger.LogInformation("Container {ContainerName} ready", containerName);
    }

    private async Task SeedInitialData()
    {
        //ToDo: Have to do this better with Aspire. It is currently empty
        var databaseName = configuration["CosmosDb:DatabaseName"] ?? "task-management-db";
        var containerName = configuration["CosmosDb:ContainerName"] ?? nameof(Category);

        var container = cosmosClient.GetContainer(databaseName, containerName);
        
        var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c");
        var count = (await container.GetItemQueryIterator<int>(query).ReadNextAsync()).FirstOrDefault();

        if (count > 0)
        {
            return;
        }

        logger.LogInformation("Seeding initial projects.");

        var categories = new[]
        {
            new Category {
                Name = "Personal",
                Description = "Tasks related to personal life",
                Tasks = new List<TaskItem>()
                {
                    new TaskItem()
                    {
                        Title = "Renew Car License",
                        Description = "Renew car license before expiry which is on 15th of this month",
                        DueDate = DateTime.UtcNow.AddDays(7),
                        Status = TaskItemStatus.Todo,
                        CreatedAt = DateTime.UtcNow,
                    }
                },
                CreatedAt = DateTime.UtcNow
            },
            new Category {
                Name = "Work",
                Description = "Tasks related to work",
                Tasks = new List<TaskItem>(),
                CreatedAt = DateTime.UtcNow
            }
        };

        foreach (var category in categories)
        {
            await container.CreateItemAsync(category);
        }
    }
}