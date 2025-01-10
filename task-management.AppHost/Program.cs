var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("cosmos-db")
                    .RunAsEmulator(emulator =>
                    {
                        emulator.WithGatewayPort(7777);
                    })
                    ;

var apiService = builder.AddProject<Projects.task_management_ApiService>("apiservice")
        .WithReference(cosmos)
        .WaitFor(cosmos);

builder.AddProject<Projects.task_management_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
