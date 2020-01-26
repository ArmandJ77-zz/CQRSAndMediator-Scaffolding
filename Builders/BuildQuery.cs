using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using System.Collections.Generic;
using CQRSAndMediator.Scaffolding.Models;

namespace CQRSTemplates.Builders
{
    public static class BuildQuery
    {
        public static void Build(string concern, string operation)
        {
            ClassAssembler
                .Configure(concern,operation, PatternDirectoryType.Queries)
                .ImportNamespaces(new List<NamespaceModel>
                {
                    new NamespaceModel("MediatR"),
                    new NamespaceModel($"Responses.{concern}",true)
                })
                .CreateNamespace()
                .CreateClass()
                .WithInheritance(new List<string>
                {
                     $"IRequest<{concern}{operation}{PatternFileType.Response}>"
                })
                .Generate()
                ;
        }
    }
}
