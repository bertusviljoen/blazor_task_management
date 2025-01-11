namespace task_management.Shared.Models;

public record CategoryDto(
    Guid Id,
    string Name,
    string Description,
    List<TaskItem> Tasks,
    DateTime CreatedAt,
    string? Etag = null);

public class SelectedCategoryDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public DateTime CreatedAt { get; set; }
    public string? Etag { get; set; } = null;
    
    public SelectedCategoryDto(CategoryDto category)
    {
        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
        Tasks = category.Tasks;
        CreatedAt = category.CreatedAt;
        Etag = category.Etag;
    }

    public SelectedCategoryDto()
    {
        
    }
    
    public CategoryDto ToCategoryDto()
    {
        return new CategoryDto(Id, Name, Description, Tasks, CreatedAt, Etag);
    }
}
