using System.Collections.Generic;
using System.Threading.Tasks;
using task_management.Shared.Models;

namespace task_management.Web.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(Guid id);
    Task UpdateCategoryAsync(Guid id, CategoryDto category);
    Task CreateCategoryAsync(CategoryDto category);
    Task DeleteCategoryAsync(Guid id);
}
