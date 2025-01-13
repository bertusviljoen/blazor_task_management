using task_management.Shared.Models;

namespace task_management.Web.Components.Pages;

public record TaskEditModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskItemPriority Priority { get; set; }
    public TaskItemStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
}