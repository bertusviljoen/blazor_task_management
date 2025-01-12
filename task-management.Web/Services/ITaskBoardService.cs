using System.Collections.Generic;
using System.Threading.Tasks;
using task_management.Shared.Models;

namespace task_management.Web.Services;

public interface ITaskBoardService
{
    Task<IEnumerable<TaskBoardDto>> GetAllTaskBoardsAsync();
    Task<TaskBoardDto> GetTaskBoardByIdAsync(Guid id);
    Task UpdateTaskBoardAsync(Guid id, TaskBoardDto taskBoard);
    Task CreateTaskBoardAsync(TaskBoardDto taskBoard);
    Task DeleteTaskBoardAsync(Guid id);
}
