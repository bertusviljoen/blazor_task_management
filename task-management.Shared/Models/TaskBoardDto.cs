namespace task_management.Shared.Models;

public record TaskBoardDto(
    Guid Id,
    string Name,
    string Description,
    List<TaskItem> Tasks,
    DateTime CreatedAt,
    string? Etag = null);

public class SelectedTaskBoardDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public DateTime CreatedAt { get; set; }
    public string? Etag { get; set; } = null;
    
    public SelectedTaskBoardDto(TaskBoardDto taskBoard)
    {
        Id = taskBoard.Id;
        Name = taskBoard.Name;
        Description = taskBoard.Description;
        Tasks = taskBoard.Tasks;
        CreatedAt = taskBoard.CreatedAt;
        Etag = taskBoard.Etag;
    }

    public SelectedTaskBoardDto()
    {
        
    }
    
    public TaskBoardDto ToTaskBoardDto()
    {
        return new TaskBoardDto(Id, Name, Description, Tasks, CreatedAt, Etag);
    }
}
