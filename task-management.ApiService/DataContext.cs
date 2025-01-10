using Microsoft.EntityFrameworkCore;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<task_management.ApiService.Models.Project> Project { get; set; } = default!;
}
