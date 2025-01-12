namespace task_management.Web.Components.Pages;

public record TaskBoardEditModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
