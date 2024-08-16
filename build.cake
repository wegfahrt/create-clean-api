//#addin nuget:?package=Cake.Powershell&version=2.0.0

var solution = "CleanArchitectureExample";
var projectNames = new[] { "Domain", "Application", "Infrastructure", "WebApi", "Tests" };
var target = Argument("target", "Default");

Task("Clean")
    .Does(() =>
    {
        Information("Should clean stuff");
        CleanDirectories("./**/bin");
        CleanDirectories("./**/obj");

        CleanDirectories("./src");
        var solutionFile = GetFiles("./CleanArchitectureExample.sln");
        DeleteFiles(solutionFile);
    });

Task("Create-Solution-And-Projects")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        //StartProcess("dotnet", new ProcessSettings { Arguments = $"new sln -n {solution}" });
        StartProcess("dotnet", new ProcessSettings { Arguments = $"new sln --name {solution}" });

        foreach(var project in projectNames)
        {
            var projectType = project == "WebApi" ? "webapi" : project == "Tests" ? "xunit" : "classlib";
            StartProcess("dotnet", new ProcessSettings { Arguments = $"new {projectType} -n {project} -o src/{project}" });
            //TODO: platform independent?
            StartProcess("dotnet", new ProcessSettings { Arguments = $"sln add src/{project}/{project}.csproj" });
        }
        //StartProcess("dotnet", new ProcessSettings { Arguments = $"sln add **/**.csproj)" });
    });

Task("Add-Project-References")
    .IsDependentOn("Create-Solution-And-Projects")
    .Does(() => 
    {
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Application/Application.csproj reference src/Domain/Domain.csproj" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Infrastructure/Infrastructure.csproj reference src/Application/Application.csproj" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/WebApi/WebApi.csproj reference src/Application/Application.csproj" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/WebApi/WebApi.csproj reference src/Infrastructure/Infrastructure.csproj" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Tests/Tests.csproj reference src/Domain/Domain.csproj" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Tests/Tests.csproj reference src/Application/Application.csproj" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Tests/Tests.csproj reference src/Infrastructure/Infrastructure.csproj" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Tests/Tests.csproj reference src/WebApi/WebApi.csproj" });
    });

Task("Add-Packages")
    .IsDependentOn("Add-Project-References")
    .Does(() =>
    {
        // Domain
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Domain/Domain.csproj package ErrorOr" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Domain/Domain.csproj package Throw" });
        
        // Application
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Application/Application.csproj package MediatR" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Application/Application.csproj package ErrorOr" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Application/Application.csproj package Throw" });
        
        // Infrastructure
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore" });
        //StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore.InMemory" });
        
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Infrastructure/Infrastructure.csproj package Microsoft.Extensions.Configuration" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Infrastructure/Infrastructure.csproj package Microsoft.Configuration" });
        
        // WebApi
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/WebApi/WebApi.csproj package MediatR" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/WebApi/WebApi.csproj package ErrorOr" });
        
        // Tests
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Tests/Tests.csproj package FluentAssertions" });
        StartProcess("dotnet", new ProcessSettings { Arguments = "add src/Tests/Tests.csproj package Moq" });
    });

Task("Create-Example-Files")
    .IsDependentOn("Add-Packages")
    .Does(() =>
    {
        EnsureDirectoryExists("src/Domain/Entities");
        CopyFile("templates/Domain/User.cs", "src/Domain/Entities/User.cs");

        EnsureDirectoryExists("src/Application/Interfaces");
        CopyFile("templates/Application/IUserRepository.cs", "src/Application/Interfaces/IUserRepository.cs");
        CopyFile("templates/Application/DependencyInjection.cs", "src/Application/DependencyInjection.cs");

        EnsureDirectoryExists("src/Application/Users/Queries");
        CopyFile("templates/Application/GetUserQuery.cs", "src/Application/Users/Queries/GetUserQuery.cs");
        CopyFile("templates/Application/GetUserQueryHandler.cs", "src/Application/Users/Queries/GetUserQueryHandler.cs");

        EnsureDirectoryExists("src/Application/Common/Interfaces");
        CopyFile("templates/Application/IUnitOfWork.cs", "src/Application/Common/Interfaces/IUnitOfWork.cs");

        EnsureDirectoryExists("src/Infrastructure/Repositories");
        CopyFile("templates/Infrastructure/UserRepository.cs", "src/Infrastructure/Repositories/UserRepository.cs");
        CopyFile("templates/Infrastructure/DependencyInjection.cs", "src/Infrastructure/DependencyInjection.cs");

        EnsureDirectoryExists("src/Infrastructure/Common/Persistence");
        CopyFile("templates/Infrastructure/AppDbContext.cs", "src/Infrastructure/Common/Persistence/AppDbContext.cs");

        EnsureDirectoryExists("src/WebApi/Controllers");
        CopyFile("templates/WebApi/UsersController.cs", "src/WebApi/Controllers/UsersController.cs");
        CopyFile("templates/WebApi/Program.cs", "src/WebApi/Program.cs");

        CopyFile("templates/Tests/GetUserQueryHandlerTests.cs", "src/Tests/GetUserQueryHandlerTests.cs");

        EnsureDirectoryExists("src/Infrastructure/ExampleUtil");
        CopyFile("templates/ExampleUtil/DbInitalizer.cs", "src/Infrastructure/ExampleUtil/DbInitializer.cs");
    });

Task("Delete-Default-Classes")
    .IsDependentOn("Create-Example-Files")
    .Does(() =>
    {
        var classlibFiles = GetFiles("./src/**/Class1.cs");
        DeleteFiles(classlibFiles);
        var xunitFiles = GetFiles("./src/**/UnitTest1.cs");
        DeleteFiles(xunitFiles);
    });

Task("Default")
    .IsDependentOn("Delete-Default-Classes");

RunTarget(target);
