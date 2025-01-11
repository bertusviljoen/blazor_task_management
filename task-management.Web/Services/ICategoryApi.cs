using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using task_management.Shared.Models;

namespace task_management.Web.Services;

public interface ICategoryApi
{
    [Get("/api/Category")]
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken token = default);

    [Get("/api/Category/{id}")]
    Task<CategoryDto> GetCategoryByIdAsync(string id, CancellationToken token = default);

    [Put("/api/Category/{id}")]
    Task UpdateCategoryAsync(string id, [Body] CategoryDto category, CancellationToken token = default);

    [Post("/api/Category")]
    Task CreateCategoryAsync([Body] CategoryDto category, CancellationToken token = default);

    [Delete("/api/Category/{id}")]
    Task DeleteCategoryAsync(string id, CancellationToken token = default);
}
