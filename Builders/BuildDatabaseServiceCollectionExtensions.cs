using System;
using CQRSAndMediator.Scaffolding.Models;
using CQRSAndMediator.Scaffolding.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using CQRSAndMediator.Scaffolding.Infrastructure;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildDatabaseServiceCollectionExtensions
    {
        public static void Build(string path)
        {
            Console.WriteLine(ExecuteCommandUtility.Run($"echo [92mADD DATABASE SERVICE COLLECTION EXTENSION[0m"));
            const string className = "DatabaseServiceCollectionExtensions";
            const string methodName = "AddDatabase";
            const string nameSpace = "DB.Configuration";

            Directory.CreateDirectory(path);

            ExecuteCommandUtility.Run("cd DB/Configuration");

            var importNameSpaces = new List<NamespaceModel>
            {
                new NamespaceModel("Microsoft.Extensions.Configuration"),
                new NamespaceModel("Microsoft.Extensions.DependencyInjection"),
            };

            var modifiers = new[]
            {
                SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                ,SyntaxFactory.Token(SyntaxKind.StaticKeyword)
            };

            var statements = new List<StatementSyntax>
            {
                SyntaxFactory.ParseStatement("return services;")
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
            };

            var paramArray = new[]
            {
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("services"))
                .WithType(SyntaxFactory.ParseTypeName("this IServiceCollection")),
                SyntaxFactory.Parameter(SyntaxFactory.Identifier("config"))
                .WithType(SyntaxFactory.ParseTypeName("IConfiguration"))
            };

            ClassAssembler
                .Configure()
                .ImportNamespaces(importNameSpaces)
                .CreateNamespace(nameSpace)
                .CreateClass(modifiers, className)
                .AddMethod(
                    modifiers
                    , SyntaxFactory.ParseTypeName($"IServiceCollection")
                    , methodName
                    , paramArray
                    , statements)
                .Generate(path, className);

            ExecuteCommandUtility.Run("cd ../../");
        }
    }
}
