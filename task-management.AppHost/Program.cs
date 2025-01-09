var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.task_management_ApiService>("apiservice");

builder.AddProject<Projects.task_management_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
