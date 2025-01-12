using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using task_management.Shared.Models;

namespace task_management.Web.Services;

public class ClientTaskBoardService(HttpClient http) : ITaskBoardService
{
    public async Task<IEnumerable<TaskBoardDto>> GetAllTaskBoardsAsync()
    {
        return await http.GetFromJsonAsync<IEnumerable<TaskBoardDto>>("api/TaskBoard") ?? [];
    }

    public async Task<TaskBoardDto> GetTaskBoardByIdAsync(Guid id)
    {
        var response = await http.GetAsync($"api/TaskBoard/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TaskBoardDto>()
            ?? throw new Exception("TaskBoard not found");
    }

    public async Task UpdateTaskBoardAsync(Guid id, TaskBoardDto taskBoard)
    {
        await http.PutAsJsonAsync($"api/TaskBoard/{id}", taskBoard);
    }

    public async Task CreateTaskBoardAsync(TaskBoardDto taskBoard)
    {
        await http.PostAsJsonAsync("api/TaskBoard", taskBoard);
    }

    public async Task DeleteTaskBoardAsync(Guid id)
    {
        await http.DeleteAsync($"api/TaskBoard/{id}");
    }
}
