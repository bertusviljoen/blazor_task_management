//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.EntityFrameworkCore;
//using task_management.ApiService.Models;

//public static class ProjectsEndpoint
//{
//    public static void MapProjectEndpoints(this IEndpointRouteBuilder routes)
//    {
//        var group = routes.MapGroup("/api/Project").WithTags(nameof(Project));

//        group.MapGet("/", async (DataContext db) =>
//        {
//            return await db.Project.ToListAsync();
//        })
//        .WithName("GetAllProjects")
//        .WithOpenApi();

//        group.MapGet("/{id}", async Task<Results<Ok<Project>, NotFound>> (int id, DataContext db) =>
//        {
//            return await db.Project.AsNoTracking()
//                .FirstOrDefaultAsync(model => model.Id == id)
//                is Project model
//                    ? TypedResults.Ok(model)
//                    : TypedResults.NotFound();
//        })
//        .WithName("GetProjectById")
//        .WithOpenApi();

//        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, Project project, DataContext db) =>
//        {
//            var foundModel = await db.Project.FindAsync(id);

//            if (foundModel is null)
//            {
//                return TypedResults.NotFound();
//            }

//            db.Update(project);
//            await db.SaveChangesAsync();

//            return TypedResults.NoContent();
//        })
//        .WithName("UpdateProject")
//        .WithOpenApi();

//        group.MapPost("/", async (Project project, DataContext db) =>
//        {
//            db.Project.Add(project);
//            await db.SaveChangesAsync();
//            return TypedResults.Created($"/api/Project/{project.Id}",project);
//        })
//        .WithName("CreateProject")
//        .WithOpenApi();

//        group.MapDelete("/{id}", async Task<Results<Ok<Project>, NotFound>> (int id, DataContext db) =>
//        {
//            if (await db.Project.FindAsync(id) is Project project)
//            {
//                db.Project.Remove(project);
//                await db.SaveChangesAsync();
//                return TypedResults.Ok(project);
//            }

//            return TypedResults.NotFound();
//        })
//        .WithName("DeleteProject")
//        .WithOpenApi();
//    }
//}
