using Microsoft.AspNetCore.Http.HttpResults;
using task_management.ApiService.Infrastructure;
using task_management.ApiService.Models;
using task_management.Shared.Models;

public static class TaskBoardEndpoint
{
    public static void MapTaskBoardEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TaskBoard").WithTags(nameof(TaskBoard));

        group.MapGet("/", async (IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<TaskBoard>();
            var items = await repository.GetAllAsync((taskBoard => Guid.Empty != taskBoard.Id), token);
            return TypedResults.Ok(items.Select(a => new TaskBoardDto(a.Id, a.Name, a.Description, a.Tasks, a.CreatedAt, a._etag)));
        })
        .WithName("GetAllTaskBoards")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<TaskBoardDto>, NotFound>> (string id, IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<TaskBoard>();
            var taskBoard = await repository.GetAsync(id, token);
            return taskBoard is not null
                ? TypedResults.Ok(new TaskBoardDto(taskBoard.Id, taskBoard.Name, taskBoard.Description, taskBoard.Tasks, taskBoard.CreatedAt, taskBoard._etag))
                : TypedResults.NotFound();
        })
        .WithName("GetTaskBoardById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (string id, TaskBoardDto taskBoard, IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<TaskBoard>();
            var existingTaskBoard = await repository.GetAsync(id, token);
            if (existingTaskBoard is null)
            {
                return TypedResults.NotFound();
            }
            
            existingTaskBoard.Name = taskBoard.Name;
            existingTaskBoard.Description = taskBoard.Description;
            existingTaskBoard.Tasks = taskBoard.Tasks;
            existingTaskBoard.CreatedAt = taskBoard.CreatedAt;
            existingTaskBoard._etag = taskBoard.Etag;
            
            await repository.UpdateAsync(existingTaskBoard, token);
            return TypedResults.NoContent();
        })
        .WithName("UpdateTaskBoard")
        .WithOpenApi();

        group.MapPost("/", async (TaskBoardDto taskBoard, IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<TaskBoard>();
            
            var newTaskBoard = new TaskBoard
            {
                Id = taskBoard.Id,
                Name = taskBoard.Name,
                Description = taskBoard.Description,
                Tasks = taskBoard.Tasks,
                CreatedAt = taskBoard.CreatedAt
            };
            
            await repository.AddAsync(newTaskBoard, token);
            return TypedResults.Created($"/api/TaskBoard/{taskBoard.Id}", taskBoard);
        })
        .WithName("CreateTaskBoard")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok<TaskBoardDto>, NotFound>> (string id, IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<TaskBoard>();
            var taskBoard = await repository.GetAsync(id, token);
            if (taskBoard is null)
            {
                return TypedResults.NotFound();
            }
            await repository.DeleteAsync(id, token);
            return TypedResults.Ok(new TaskBoardDto(taskBoard.Id, taskBoard.Name, taskBoard.Description, taskBoard.Tasks, taskBoard.CreatedAt));
        })
        .WithName("DeleteTaskBoard")
        .WithOpenApi();
    }
}