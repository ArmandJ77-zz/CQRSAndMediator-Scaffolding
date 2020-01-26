using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Models;
using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildHandler
    {
        public static void Build(string concern, string operation, OperationType ot)
        {
            var tIn = ot switch
            {
                OperationType.COMMAND => $"{concern}{operation}Command",
                OperationType.QUERY => $"{concern}{operation}Query"
            };

            var tInNamespace = ot switch
            {
                OperationType.COMMAND => $"Commands.{concern}",
                OperationType.QUERY => $"Queries.{concern}"
            };
            
            ClassAssembler
                .Configure(concern, operation, PatternDirectoryType.Handlers)
                .ImportNamespaces(new List<NamespaceModel>
                {
                    new NamespaceModel("MediatR"),
                    new NamespaceModel(tInNamespace,true),
                    new NamespaceModel($"Responses.{concern}",true),
                    new NamespaceModel("System.Collections.Generic"),
                    new NamespaceModel("System.Threading"),
                    new NamespaceModel("System.Threading.Tasks")
                })
                .CreateNamespace()
                .CreateClass()
                .WithInheritance(new List<string>
                {
                    $"IRequestHandler<{tIn},{concern}{operation}Response>"
                })
                .ImplementMediatorHandlerInheritance($"{concern}{operation}Response",tIn)
                .Generate()
                ;
        }
    }
}
