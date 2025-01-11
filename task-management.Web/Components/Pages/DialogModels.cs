namespace task_management.Web.Components.Pages;

public record CategoryEditModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
