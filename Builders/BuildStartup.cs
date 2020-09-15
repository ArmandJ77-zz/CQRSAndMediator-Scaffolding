using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Models;
using CQRSAndMediator.Scaffolding.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildStartup
    {
        private static List<NamespaceModel> _namespaceModels = new List<NamespaceModel>
        {
            new NamespaceModel("Api.Extensions"),
            new NamespaceModel("DB.Configuration"),
            new NamespaceModel("Logic.Configuration"),
            new NamespaceModel("Microsoft.AspNetCore.Builder"),
            new NamespaceModel("Microsoft.AspNetCore.Hosting"),
            new NamespaceModel("Microsoft.Extensions.Configuration"),
            new NamespaceModel("Microsoft.Extensions.DependencyInjection"),
            new NamespaceModel("Microsoft.Extensions.Hosting"),
        };

        public static void Build(string path)
        {
            Console.WriteLine(ExecuteCommandUtility.Run($"echo [92mADD STARTUP[0m"));
            ExecuteCommandUtility.Run("rm Startup.cs");

            ClassAssembler
                .Configure()
                .ImportNamespaces(_namespaceModels)
                .CreateNamespace("Api")
                .CreateClass(new[] { SyntaxFactory.Token(SyntaxKind.PublicKeyword) }, "Startup")
                .AddGetProperty("IConfiguration", "Configuration", SyntaxKind.PrivateKeyword)
                .AddStartupConstructor()
                .AddStartupConfigureServices()
                .AddStartupConfigure()
                .Generate(path, "Startup")
                ;
        }
    }
}
