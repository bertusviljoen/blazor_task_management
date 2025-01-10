using Microsoft.Azure.Cosmos;
using task_management.ApiService.Models;

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
        var containerName = configuration["CosmosDb:ContainerName"] ?? "Project";

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

        var databaseName = configuration["CosmosDb:DatabaseName"] ?? "task-management-db";
        var containerName = configuration["CosmosDb:ContainerName"] ?? "Project";

        var container = cosmosClient.GetContainer(databaseName, containerName);


        // Check if container is empty
        var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c");
        var count = (await container.GetItemQueryIterator<int>(query).ReadNextAsync()).FirstOrDefault();

        if (count > 0)
        {
            return;
        }

        logger.LogInformation("Seeding initial projects.");

        var projects = new[]
        {
            new Project {
                Id = 1,
                Name = "Website Redesign",
                Description = "Redesign company website with modern look and feel",
                Tasks = new List<TaskItem>(),
                CreatedAt = DateTime.UtcNow
            },
            new Project {
                Id = 2,
                Name = "Mobile App Development",
                Description = "Develop cross-platform mobile application",
                Tasks = new List<TaskItem>(),
                CreatedAt = DateTime.UtcNow
            },
            new Project {
                Id = 3,
                Name = "API Migration",
                Description = "Migrate legacy APIs to microservices architecture",
                Tasks = new List<TaskItem>(),
                CreatedAt = DateTime.UtcNow
            }
        };

        foreach (var project in projects)
        {
            await container.CreateItemAsync(project, new PartitionKey(project.Id.ToString()));
        }
    }
}
