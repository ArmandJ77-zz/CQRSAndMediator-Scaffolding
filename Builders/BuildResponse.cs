using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using Microsoft.CodeAnalysis.CSharp;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildResponse
    {
        public static void Build(string concern, string operation, GroupByType groupBy)
        {
            ClassAssembler
                .ConfigureHandler(concern, operation, PatternDirectoryType.Responses, groupBy)
                .ImportNamespaces()
                .CreateNamespace()
                .CreateClass(new[] { SyntaxFactory.Token(SyntaxKind.PublicKeyword) })
                .GenerateHandler()
                ;
        }
    }
}
