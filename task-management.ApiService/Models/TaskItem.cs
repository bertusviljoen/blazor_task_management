namespace task_management.ApiService.Models;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public required DateTime DueDate { get; set; }
    public TaskItemStatus ItemStatus { get; set; } = TaskItemStatus.Todo;
    public TaskItemPriority Priority { get; set; } = TaskItemPriority.Low;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
