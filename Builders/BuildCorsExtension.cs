using System;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Models;
using CQRSAndMediator.Scaffolding.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildCorsExtension
    {
        public static void Build(string path)
        {
            Console.WriteLine(ExecuteCommandUtility.Run($"echo [92mADD CORS EXTENSION[0m"));
            const string className = "CorsServiceCollectionExtensions";
            const string methodName = "AddCorsRules";

            Directory.CreateDirectory(path);
            ExecuteCommandUtility.Run("cd Extensions");

            var importNameSpaces = new List<NamespaceModel>
            {
                new NamespaceModel("Microsoft.Extensions.DependencyInjection")
            };

            var statements = new List<StatementSyntax>
            {
                SyntaxFactory.ParseStatement("return services;")
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed),
            };
            var modifiers = new[]
            {
                SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                ,SyntaxFactory.Token(SyntaxKind.StaticKeyword)
            };
            var paramArray = new[] { SyntaxFactory.Parameter(SyntaxFactory.Identifier("services"))
                .WithType(SyntaxFactory.ParseTypeName("this IServiceCollection"))};

            ClassAssembler
                .Configure()
                .ImportNamespaces(importNameSpaces)
                .CreateNamespace("Api.Extensions")
                .CreateClass(modifiers, className)
                .AddMethod(
                    modifiers
                    , SyntaxFactory.ParseTypeName($"IServiceCollection")
                    , methodName
                    , paramArray
                    , statements)
                .Generate(path, className);


            ExecuteCommandUtility.Run("cd ../");
        }
    }
}
