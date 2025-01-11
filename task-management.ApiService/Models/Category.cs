using Newtonsoft.Json;

namespace task_management.ApiService.Models;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TaskItem> Tasks { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public string PartitionKey => Id.ToString();
    public string id => Id.ToString();
    
    public string _etag { get; set; }
}

/// <summary> Category Data Transfer Object for the API </summary>
public record CategoryDto(Guid Id,string Name, string Description, List<TaskItem> Tasks, DateTime CreatedAt, string? Etag = null);
