using Microsoft.AspNetCore.Http.HttpResults;
using task_management.ApiService.Infrastructure;
using task_management.ApiService.Models;
using task_management.Shared.Models;

public static class CategoriesEndpoint
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Category").WithTags(nameof(Category));

        group.MapGet("/", async (IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<Category>();
            var items = await repository.GetAllAsync((category => Guid.Empty != category.Id), token);
            return TypedResults.Ok(items.Select(a => new CategoryDto(a.Id, a.Name, a.Description, a.Tasks, a.CreatedAt, a._etag)));
        })
        .WithName("GetAllCategories")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<CategoryDto>, NotFound>> (string id, IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<Category>();
            var category = await repository.GetAsync(id, token);
            return category is not null
                ? TypedResults.Ok(new CategoryDto(category.Id, category.Name, category.Description, category.Tasks, category.CreatedAt, category._etag))
                : TypedResults.NotFound();
        })
        .WithName("GetCategoryById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (string id, CategoryDto category, IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<Category>();
            var foundModel = await repository.GetAsync(id, token);
            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }
            
            foundModel.Name = category.Name;
            foundModel.Description = category.Description;
            foundModel.Tasks = category.Tasks;
            foundModel.CreatedAt = category.CreatedAt;
            foundModel._etag = category.Etag;
            
            await repository.UpdateAsync(foundModel, token);
            return TypedResults.NoContent();
        })
        .WithName("UpdateCategory")
        .WithOpenApi();

        group.MapPost("/", async (CategoryDto category, IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<Category>();
            
            var newCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Tasks = category.Tasks,
                CreatedAt = category.CreatedAt
            };
            
            await repository.AddAsync(newCategory, token);
            return TypedResults.Created($"/api/Category/{category.Id}", category);
        })
        .WithName("CreateCategory")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok<CategoryDto>, NotFound>> (string id, IDataContext db, CancellationToken token) =>
        {
            var repository = db.Set<Category>();
            var category = await repository.GetAsync(id, token);
            if (category is null)
            {
                return TypedResults.NotFound();
            }
            await repository.DeleteAsync(id, token);
            return TypedResults.Ok(new CategoryDto(category.Id, category.Name, category.Description, category.Tasks, category.CreatedAt));
        })
        .WithName("DeleteCategory")
        .WithOpenApi();
    }
}