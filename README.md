# blazor_task_management

Sample application for task management using Blazor and FluentUI.

## Project Scope

The project scope for the `blazor_task_management` repository is to create a sample application for task management using Blazor and FluentUI. The project involves the following components and technologies:

* **Blazor**: A framework for building interactive web UIs using C# and .NET.
* **FluentUI**: A set of UI components for building web applications with a consistent look and feel.
* **Aspire**: A set of tools and libraries for building distributed applications.
* **API Service**: A backend service that provides data and functionality to the frontend.
* **Render Mode**: The application uses interactive server render mode for Blazor components.

## Project Structure

The project structure includes the following main components:

* `task-management.ApiService`: Contains the backend API service.
* `task-management.AppHost`: Hosts the application and manages the different services.
* `task-management.Web`: Contains the Blazor web frontend.
* `task-management.ServiceDefaults`: Provides common service configurations and integrations.

## Technologies in Play

* **Blazor**: Used for the frontend.
* **FluentUI**: Used for UI components.
* **Aspire**: Used for service defaults and client integrations.
* **API**: Used for backend services.
* **Render Mode**: Used in Blazor components.

## SDK and Version

* **SDK**: .NET SDK
* **Version**: 9.0

## How to Checkout and Run

1. **Clone the repository**:
   ```sh
   git clone https://github.com/bertusviljoen/blazor_task_management.git
   cd blazor_task_management
   ```

2. **Install the .NET SDK**:
   Ensure you have the .NET SDK version 9.0 installed. You can download it from the official [.NET website](https://dotnet.microsoft.com/download).

3. **Restore the dependencies**:
   ```sh
   dotnet restore
   ```

4. **Build the solution**:
   ```sh
   dotnet build
   ```

5. **Run the application**:
   ```sh
   dotnet run --project task-management.AppHost
   ```

6. **Open the application**:
   Open your browser and navigate to `https://localhost:17138` or `http://localhost:15156` to access the application.

7. Import cosmos certificate

   ```sh
   curl --insecure <https://localhost:7777/_explorer/emulator.pem> > .emulatorcert.crt
   ```
8. Trust the certificate

   ```sh
   Import-Certificate -FilePath .emulatorcert.crt -CertStoreLocation 'Cert:\CurrentUser\Root'
   ```

   Note that a confirmation dialog will appear. Click Yes to trust the certificate.

9. You can now access the emulator explorer at:    https://localhost:7777/_explorer/index.html
