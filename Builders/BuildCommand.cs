using System.Collections.Generic;
using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Models;
using Microsoft.CodeAnalysis.CSharp;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildCommand
    {
        public static void Build(string concern, string operation, GroupByType groupBy)
        {
            var responseNameSpace = groupBy switch
            {
                GroupByType.Concern => $"{concern}.Responses",
                GroupByType.Operation => $"Responses.{concern}"
            };

            ClassAssembler
                .ConfigureHandler(concern, operation, PatternDirectoryType.Commands, groupBy)
                .ImportNamespaces(new List<NamespaceModel>
                {
                    new NamespaceModel("MediatR"),
                    new NamespaceModel(responseNameSpace,true)
                })
                .CreateNamespace()
                .CreateClass(new []{SyntaxFactory.Token(SyntaxKind.PublicKeyword)})
                .WithInheritance(new List<string>
                {
                    $"IRequest<{concern}{operation}{PatternFileType.Response}>"
                })
                .GenerateHandler()
                ;
        }
    }
}
