using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Models;
using CQRSAndMediator.Scaffolding.Resolver;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildQuery
    {
        public static void Build(string concern, string operation, GroupByType groupBy)
        {
            ClassAssembler
                .ConfigureHandler(concern,operation, PatternDirectoryType.Queries, groupBy)
                .ImportNamespaces(new List<NamespaceModel>
                {
                    new NamespaceModel("MediatR"),
                    new NamespaceModel($"{NamespaceResolver.Resolve(concern,"Responses",groupBy)}",true),
                })
                .CreateNamespace()
                .CreateClass(new[] { SyntaxFactory.Token(SyntaxKind.PublicKeyword) })
                .WithInheritance(new List<string>
                {
                     $"IRequest<{concern}{operation}{PatternFileType.Response}>"
                })
                .GenerateHandler()
                ;
        }
    }
}
