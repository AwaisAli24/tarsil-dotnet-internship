# TodoApi Project Structure

## Main Files and Folders

- **`Program.cs`**: The application's entry point. It registers services, configures middleware and endpoints, and starts the server.
- **`Controllers/`**: Contains controller classes that handle HTTP requests, such as `GET /api/todos` and `POST /api/todos`.
- **`appsettings.json`**: Stores application configuration, including logging settings and other configuration values.
- **`appsettings.Development.json`**: Stores configuration specific to the Development environment. Its values can override corresponding settings in `appsettings.json`.
- **`Properties/`**: Contains development and project-related properties.
- **`Properties/launchSettings.json`**: Defines local launch profiles, environment variables, and development URLs.
- **`TodoApi.csproj`**: Defines the target .NET framework, project settings, and package references.
- **`TodoApi.http`**: Contains HTTP requests that can be executed from a supported editor to test the API.
- **`bin/`**: Contains compiled build output.
- **`obj/`**: Contains intermediate files generated during the build process.

## Program.cs: Line-by-Line Explanation

### 1. Create the application builder

```csharp
var builder = WebApplication.CreateBuilder(args);
```

Creates the application builder and initializes configuration, logging, dependency injection, and the hosting environment.

`args` contains command-line arguments passed to the application.

### 2. Register controller services

```csharp
builder.Services.AddControllers();
```

Registers the services required for controller-based APIs, allowing ASP.NET Core to discover and execute controller actions.

### 3. Register OpenAPI services

```csharp
builder.Services.AddOpenApi();
```

Registers the services needed to generate an OpenAPI document describing the API.

The document describes available endpoints, HTTP methods, request parameters, request bodies, and response types. Tools can use this information to understand and test the API.

### 4. Build the application

```csharp
var app = builder.Build();
```

Builds the application using the services and configuration registered through `builder`.

After this line, the application is ready to be configured with middleware and endpoints.

### 5. Configure HTTPS redirection

```csharp
app.UseHttpsRedirection();
```

Adds middleware that redirects eligible HTTP requests to HTTPS when HTTPS redirection is configured.

This helps clients communicate with the API over an encrypted connection.

### 6. Check the hosting environment

```csharp
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
```

`app.Environment.IsDevelopment()` checks whether the application is running in the `Development` environment.

If the condition is true, the code inside the block executes.

`app.MapOpenApi()` maps an endpoint that serves the generated OpenAPI JSON document. With the default configuration, this is typically available at:

`/openapi/v1.json`

Keeping this endpoint inside the condition means the OpenAPI document is exposed only when the application runs in the Development environment.

### 7. Configure authorization middleware

```csharp
app.UseAuthorization();
```

Adds authorization middleware, which checks whether a request is allowed to access an endpoint when authorization policies apply.

This line alone does not configure authentication or require authorization for every endpoint.

### 8. Map controller endpoints

```csharp
app.MapControllers();
```

Maps the routes defined by controller attributes.

For example:

```csharp
[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }
}
```

The `GetAll()` action can be reached through `GET /api/todos`, assuming the controller is named `TodosController`.

### 9. Start the application

```csharp
app.Run();
```

Starts the web application and begins listening for requests. The application continues running until it is stopped.

## How an HTTP Request Reaches a Controller

1. `dotnet run` launches the application using the selected launch profile, if applicable.
2. ASP.NET Core receives an HTTP request.
3. Middleware processes the request.
4. Endpoint routing matches the URL and HTTP method to a controller action.
5. The controller action executes and returns a result.
6. ASP.NET Core sends the HTTP response back to the client.

## Important Distinction

- `AddOpenApi()` registers the services needed to generate an OpenAPI document.
- `app.Environment.IsDevelopment()` checks the current hosting environment.
- `MapOpenApi()` exposes the OpenAPI document through an HTTP endpoint.

These three statements have different responsibilities and should be understood separately.
