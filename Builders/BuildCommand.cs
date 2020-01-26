using System.Collections.Generic;
using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Models;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildCommand 
    {
        public static void Build(string concern, string operation)
        {
            ClassAssembler
                .Configure(concern,operation, PatternDirectoryType.Commands)
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
