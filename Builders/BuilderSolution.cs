using CQRSAndMediator.Scaffolding.Utilities;
using System;
using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuilderSolution
    {
        private static List<string> _commands = new List<string>
            {
                $"echo [92mCREATING LAYERS[0m",
                "dotnet new webapi -n Api",
                "dotnet new classlib -n Logic",
                "dotnet new classlib -n DB",
                "dotnet new nunit -n Unit.Tests",
                "dotnet new nunit -n Integration.Tests",

                $"echo [92mADD LAYERS TO SOLUTION[0m",
                "dotnet sln add Api/Api.csproj Logic/Logic.csproj DB/DB.csproj Unit.Tests/Unit.Tests.csproj Integration.Tests/Integration.Tests.csproj",

                $"echo [92mADD PROJECT REFERENCES[0m",
                "dotnet add Api/Api.csproj reference Logic/Logic.csproj DB/DB.csproj",
                "dotnet add Logic/Logic.csproj reference DB/DB.csproj",
                "dotnet add Unit.Tests/Unit.Tests.csproj reference Api/Api.csproj Logic/Logic.csproj DB/DB.csproj",
                "dotnet add Integration.Tests/Integration.Tests.csproj reference Api/Api.csproj Logic/Logic.csproj DB/DB.csproj",

                $"echo [92mINSTALL NUGET PACKAGES: API[0m",
                $"dotnet add  Api/Api.csproj package AspNetCore.HealthChecks.SqlServer -v 3.1.1",
                $"dotnet add  Api/Api.csproj package AspNetCore.HealthChecks.UI -v 3.1.1",
                $"dotnet add  Api/Api.csproj package AspNetCore.HealthChecks.UI.Client -v 3.1.1",
                $"dotnet add  Api/Api.csproj package AspNetCore.HealthChecks.UI.InMemory.Storage -v 3.1.1",
                $"dotnet add  Api/Api.csproj package FluentValidation -v 8.6.2",
                $"dotnet add  Api/Api.csproj package FluentValidation.AspNetCore -v 8.6.2",
                $"dotnet add  Api/Api.csproj package FluentValidation.DependencyInjectionExtensions -v 8.6.2",
                $"dotnet add  Api/Api.csproj package MediatR -v 8.0.0",
                $"dotnet add  Api/Api.csproj package MediatR.Extensions.Microsoft.DependencyInjection -v 8.0.0",
                $"dotnet add  Api/Api.csproj package Microsoft.AspNetCore.Mvc.NewtonsoftJson -v 3.5.1",
                $"dotnet add  Api/Api.csproj package Microsoft.Extensions.Logging.Debug -v 3.5.1",
                $"dotnet add  Api/Api.csproj package Newtonsoft.Json -v 12.0.3",
                "dotnet add  Api/Api.csproj package Microsoft.AspNetCore.Mvc.NewtonsoftJson -v 3.1.5",

                $"echo [92mINSTALL NUGET PACKAGES: LOGIC[0m",
                $"cd ../Logic",
                $"dotnet add Logic/Logic.csproj package FluentValidation -v 8.6.1",
                $"dotnet add Logic/Logic.csproj package MediatR -v 8.0.0",
                $"dotnet add Logic/Logic.csproj package MediatR.Extensions.Microsoft.DependencyInjection -v 8.0.0",
                $"dotnet add Logic/Logic.csproj package Microsoft.Extensions.Configuration -v 3.1.5",
                $"dotnet add Logic/Logic.csproj package Microsoft.Extensions.Configuration.Binder -v 3.1.5",

                $"echo [92mINSTALL NUGET PACKAGES: DB[0m",
                $"cd ../DB",
                $"dotnet add DB/DB.csproj package Microsoft.EntityFrameworkCore -v 3.1.5",
                $"dotnet add DB/DB.csproj package Microsoft.EntityFrameworkCore.InMemory -v 3.1.5",
                $"dotnet add DB/DB.csproj package Microsoft.EntityFrameworkCore.Relational -v 3.1.5",
                $"dotnet add DB/DB.csproj package Microsoft.EntityFrameworkCore.SqlServer -v 3.1.5",
                $"dotnet add DB/DB.csproj package Microsoft.EntityFrameworkCore.Tools -v 3.1.5",
                $"dotnet add DB/DB.csproj package Microsoft.EntityFrameworkCore.Design -v 3.1.5",
                $"dotnet add DB/DB.csproj package Microsoft.Extensions.DependencyInjection -v 3.1.5",
                $"dotnet add DB/DB.csproj package Microsoft.AspNetCore.Hosting.Abstractions -v 2.2.0",
                $"dotnet add DB/DB.csproj package Microsoft.Extensions.Options -v 3.1.5",
                $"dotnet add DB/DB.csproj package Microsoft.Extensions.Options.ConfigurationExtensions -v 3.1.5",
                $"dotnet add DB/DB.csproj package System.Data.SqlClient -v 4.8.1",

                $"echo [92mINSTALL NUGET PACKAGES: UNIT.TESTS[0m",
                $"cd ../Unit.Tests",
                $"dotnet add Unit.Tests/Unit.Tests.csproj package Microsoft.AspNetCore.Mvc.Testing -v 3.1.5",
                $"dotnet add Unit.Tests/Unit.Tests.csproj package Microsoft.AspNetCore.TestHost -v 3.1.5",
                $"dotnet add Unit.Tests/Unit.Tests.csproj package Microsoft.EntityFrameworkCore -v 3.1.5",
                $"dotnet add Unit.Tests/Unit.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory -v 3.1.5",
                $"dotnet add Unit.Tests/Unit.Tests.csproj package Microsoft.EntityFrameworkCore.Tools -v 3.1.5",
                $"dotnet add Unit.Tests/Unit.Tests.csproj package Microsoft.Extensions.Hosting -v 3.1.5",

                $"echo [92mINSTALL NUGET PACKAGES: INTEGRATION.TESTS[0m",
                $"cd ../Integration.Tests",
                $"dotnet add Integration.Tests/Integration.Tests.csproj package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore -v 3.1.5",
                $"dotnet add Integration.Tests/Integration.Tests.csproj package Microsoft.AspNetCore.Mvc.Testing -v 3.1.5",
                $"dotnet add Integration.Tests/Integration.Tests.csproj package Microsoft.AspNetCore.TestHost -v 3.1.5",
                $"dotnet add Integration.Tests/Integration.Tests.csproj package Microsoft.EntityFrameworkCore -v 3.1.5",
                $"dotnet add Integration.Tests/Integration.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory -v 3.1.5",
                $"dotnet add Integration.Tests/Integration.Tests.csproj package Microsoft.EntityFrameworkCore.Tools -v 3.1.5",
                $"dotnet add Integration.Tests/Integration.Tests.csproj package Microsoft.Extensions.Hosting -v 3.1.5",

                $"echo [92mADD GIT IGNORE[0m",
                "cd ../",
                $"gitignore VisualStudio",

                $"echo [92mCLEAN PROJECT[0m",
                $"dotnet clean",

                $"echo [92mBUILD PROJECT[0m",
                $"dotnet build"
            };

        public static void Build(string solutionName)
        {
            Console.WriteLine(ExecuteCommandUtility.Run($"echo [92mCREATING SOLUTION[0m"));
            Console.WriteLine(ExecuteCommandUtility.Run($"dotnet new sln -n {solutionName}"));

            _commands.ForEach(cmd => Console.WriteLine(ExecuteCommandUtility.Run(cmd)));
        }
    }
}
