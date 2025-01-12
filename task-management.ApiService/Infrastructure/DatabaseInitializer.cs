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
        var containerName = configuration["CosmosDb:ContainerName"] ?? nameof(TaskBoard);

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
        var containerName = configuration["CosmosDb:ContainerName"] ?? nameof(TaskBoard);

        var container = cosmosClient.GetContainer(databaseName, containerName);

        var query = new QueryDefinition("SELECT VALUE COUNT(1) FROM c");
        var count = (await container.GetItemQueryIterator<int>(query).ReadNextAsync()).FirstOrDefault();

        if (count > 0)
        {
            return;
        }

        logger.LogInformation("Seeding initial projects.");

        var taskboards = new[]
        {
            new TaskBoard {
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
                        Priority = TaskItemPriority.High,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Schedule Annual Health Checkup",
                        Description = "Book appointment with Dr. Smith for annual physical examination",
                        DueDate = DateTime.UtcNow.AddDays(14),
                        Status = TaskItemStatus.Todo,
                        Priority = TaskItemPriority.Medium,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Plan Weekend Grocery Shopping",
                        Description = "Create shopping list and buy essentials for the week",
                        DueDate = DateTime.UtcNow.AddDays(2),
                        Status = TaskItemStatus.InProgress,
                        Priority = TaskItemPriority.Low,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Pay Utility Bills",
                        Description = "Pay electricity, water, and internet bills for the month",
                        DueDate = DateTime.UtcNow.AddDays(5),
                        Status = TaskItemStatus.Todo,
                        Priority = TaskItemPriority.High,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Schedule Car Maintenance",
                        Description = "Book appointment for oil change and general maintenance check",
                        DueDate = DateTime.UtcNow.AddDays(10),
                        Status = TaskItemStatus.Todo,
                        Priority = TaskItemPriority.Medium,
                        CreatedAt = DateTime.UtcNow,
                    }
                },
                CreatedAt = DateTime.UtcNow
            },
            new TaskBoard {
                Name = "Work",
                Description = "Tasks related to work",
                Tasks = new List<TaskItem>()
                {
                    new TaskItem()
                    {
                        Title = "Complete Quarterly Report",
                        Description = "Prepare and finalize Q3 performance report for management review",
                        DueDate = DateTime.UtcNow.AddDays(3),
                        Status = TaskItemStatus.InProgress,
                        Priority = TaskItemPriority.High,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Team Performance Reviews",
                        Description = "Conduct annual performance reviews for team members",
                        DueDate = DateTime.UtcNow.AddDays(14),
                        Status = TaskItemStatus.Todo,
                        Priority = TaskItemPriority.High,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Update Project Documentation",
                        Description = "Review and update technical documentation for the current sprint",
                        DueDate = DateTime.UtcNow.AddDays(5),
                        Status = TaskItemStatus.Todo,
                        Priority = TaskItemPriority.Medium,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Client Meeting Preparation",
                        Description = "Prepare presentation and demo for upcoming client meeting",
                        DueDate = DateTime.UtcNow.AddDays(2),
                        Status = TaskItemStatus.InProgress,
                        Priority = TaskItemPriority.High,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Code Review",
                        Description = "Review pull requests from the development team",
                        DueDate = DateTime.UtcNow.AddDays(1),
                        Status = TaskItemStatus.Todo,
                        Priority = TaskItemPriority.Medium,
                        CreatedAt = DateTime.UtcNow,
                    },
                    new TaskItem()
                    {
                        Title = "Weekly Team Sync",
                        Description = "Prepare agenda and conduct weekly team status meeting",
                        DueDate = DateTime.UtcNow.AddDays(4),
                        Status = TaskItemStatus.Todo,
                        Priority = TaskItemPriority.Medium,
                        CreatedAt = DateTime.UtcNow,
                    }
                },
                CreatedAt = DateTime.UtcNow
            }
        };

        foreach (var taskBoard in taskboards)
        {
            await container.CreateItemAsync(taskBoard);
        }
    }
}
