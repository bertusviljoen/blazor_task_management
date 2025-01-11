using Newtonsoft.Json;

namespace task_management.ApiService.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TaskItem> Tasks { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public string PartitionKey => Name;
    public string id => Id.ToString();
}
