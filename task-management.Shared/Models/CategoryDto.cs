namespace task_management.Shared.Models;

public record CategoryDto(
    Guid Id,
    string Name,
    string Description,
    List<TaskItem> Tasks,
    DateTime CreatedAt,
    string? Etag = null);
