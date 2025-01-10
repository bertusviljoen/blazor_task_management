namespace task_management.ApiService.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TaskItem> Tasks { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
