# GithubActionsHelloWorld

[![.NET](https://github.com/stesee/GithubActionsHelloWorld/actions/workflows/dotnet.yml/badge.svg)](https://github.com/stesee/GithubActionsHelloWorld/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/badge/nuget-GithubActionsHelloWorld-blue)](https://int.nugettest.org/packages/GithubActionsHelloWorld)

Hello world built with github action.

## Projects

This repository contains multiple .NET 8 projects demonstrating different aspects of GitHub Actions integration:

### GithubActionsHelloWorld
Console application that generates barcodes using ZXing.Net and SixLabors.ImageSharp libraries.

### GithubActionsHelloWordWeb
ASP.NET Core minimal API web application with:
- Simple Hello World endpoint
- JSON API endpoint with timestamp
- Swagger/OpenAPI documentation
- Integration tests
- Docker support
- Azure WebDeploy deployment via GitHub Actions

### Test Projects
- **GithubActionsHelloWorldTests**: Tests for the console application
- **GithubActionsHelloWordWebTests**: Integration tests for the web application

## GitHub Actions Workflows

- **dotnet.yml**: Original workflow for building and testing
- **dotnet-web-deploy.yml**: Enhanced workflow that builds, tests, and deploys the web application to Azure using WebDeploy

## Deployment Setup

To enable Azure deployment:

1. Create an Azure Web App
2. Download the publish profile from Azure Portal (Deployment Center ? Publish Profile)
3. Add the publish profile content as a secret named `AZURE_WEBAPP_PUBLISH_PROFILE` in your GitHub repository settings
4. Update the `app-name` value in `.github/workflows/dotnet-web-deploy.yml` with your Azure Web App name
5. Push to `AspNetHelloWorld` or `main` branch to trigger deployment

## Getting Started

```bash
# Run console app
cd GithubActionsHelloWorld
dotnet run

# Run web app
cd GithubActionsHelloWordWeb  
dotnet run

# Run tests
dotnet test
