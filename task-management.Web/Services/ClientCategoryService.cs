using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using task_management.Shared.Models;

namespace task_management.Web.Services;

public class ClientCategoryService(HttpClient http) : ICategoryService
{
    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        return await http.GetFromJsonAsync<IEnumerable<CategoryDto>>("api/Category") ?? [];
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
    {
        var response = await http.GetAsync($"api/Category/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CategoryDto>()
            ?? throw new Exception("Category not found");
    }

    public async Task UpdateCategoryAsync(Guid id, CategoryDto category)
    {
        await http.PutAsJsonAsync($"api/Category/{id}", category);
    }

    public async Task CreateCategoryAsync(CategoryDto category)
    {
        await http.PostAsJsonAsync("api/Category", category);
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        await http.DeleteAsync($"api/Category/{id}");
    }
}
