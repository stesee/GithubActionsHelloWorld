# GithubActionsHelloWordWeb

ASP.NET Core minimal API web application that demonstrates:

- Simple "Hello World" endpoint at `/`
- JSON API endpoint at `/api/hello` that returns a message and timestamp
- Swagger/OpenAPI documentation available in development mode
- Full test coverage with integration tests

## Running the Application

```bash
cd GithubActionsHelloWordWeb
dotnet run
```

The application will start on `https://localhost:5001` (or check console output for the exact URL).

## Available Endpoints

- `GET /` - Returns a simple "Hello World from ASP.NET Core!" message
- `GET /api/hello` - Returns a JSON object with a message and current timestamp
- `GET /swagger` - Swagger UI (development mode only)

## Running Tests

```bash
cd GithubActionsHelloWordWebTests
dotnet test
```

## Deployment

This project includes GitHub Actions workflow for automatic deployment using WebDeploy to Azure.

### Setup Required:

1. Create an Azure Web App
2. Download the publish profile from Azure Portal
3. Add the publish profile content as a secret named `AZURE_WEBAPP_PUBLISH_PROFILE` in your GitHub repository
4. Update the `app-name` in the GitHub Actions workflow file (`.github/workflows/dotnet-web-deploy.yml`)

The deployment will automatically trigger on pushes to `AspNetHelloWorld` or `main` branches.