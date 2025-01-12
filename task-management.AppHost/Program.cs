var builder = DistributedApplication.CreateBuilder(args);

var cosmos =
        builder.AddAzureCosmosDB("cosmos-db")
                    .AddDatabase("task-management-db")
                    .RunAsEmulator(emulator =>
                    {
                        emulator.WithLifetime(ContainerLifetime.Persistent);
                        emulator.WithGatewayPort(7777);
                        emulator.WithDataVolume();
                        emulator.WithPartitionCount(100);
                    });
;

var apiService = builder.AddProject<Projects.task_management_ApiService>("apiservice")
        .WithReference(cosmos)
        .WaitFor(cosmos);

// builder.AddProject<Projects.task_management_Web>("webfrontend")
//     .WithExternalHttpEndpoints()
//     .WithReference(apiService)
//     .WaitFor(apiService);

builder.Build().Run();
