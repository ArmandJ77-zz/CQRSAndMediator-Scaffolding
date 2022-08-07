using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Models;
using CQRSAndMediator.Scaffolding.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildHealthCheckExtensions
    {
        public static void Build(string path)
        {
            Console.WriteLine(ExecuteCommandUtility.Run($"echo [92mADD HEALTH CHECK EXTENSION[0m"));
            const string className = "HealthCheckExtensions";

            Directory.CreateDirectory(path);
            ExecuteCommandUtility.Run("cd Extensions");

            var importNameSpaces = new List<NamespaceModel>
            {
                new NamespaceModel("Microsoft.AspNetCore.Builder"),
                new NamespaceModel("Microsoft.Extensions.DependencyInjection"),
            };

            var statementsOne = new List<StatementSyntax>
            {
                SyntaxFactory.ParseStatement("return services;")
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
            };
            var statementsTwo = new List<StatementSyntax>
            {
                SyntaxFactory.ParseStatement("return app;")
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
            };

            var modifiers = new[]
            {
                SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                ,SyntaxFactory.Token(SyntaxKind.StaticKeyword)
            };

            var paramArrayOne = new[] { SyntaxFactory.Parameter(SyntaxFactory.Identifier("services"))
                .WithType(SyntaxFactory.ParseTypeName("this IServiceCollection"))};

            var paramArrayTwo = new[] { SyntaxFactory.Parameter(SyntaxFactory.Identifier("app"))
                .WithType(SyntaxFactory.ParseTypeName("this IApplicationBuilder"))};

            ClassAssembler
                .Configure()
                .ImportNamespaces(importNameSpaces)
                .CreateNamespace("Api.Extensions")
                .CreateClass(modifiers, className)
                .AddMethod(
                    modifiers
                    , SyntaxFactory.ParseTypeName($"IServiceCollection")
                    , "AddMicroserviceHealthChecks"
                    , paramArrayOne
                    , statementsOne)
                .AddMethod(
                    modifiers
                    , SyntaxFactory.ParseTypeName($"IApplicationBuilder")
                    , "UseMicroserviceHealthChecks"
                    , paramArrayTwo
                    , statementsTwo
                    )
                .Generate(path, className);

        }
    }
}
