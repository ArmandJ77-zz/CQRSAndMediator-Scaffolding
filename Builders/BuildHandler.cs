using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;
using CQRSAndMediator.Scaffolding.Models;
using System.Collections.Generic;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildHandler
    {
        public static void Build(string concern, string operation, OperationType ot,  GroupByType groupBy)
        {
            var tInObjectName = ot switch
            {
                OperationType.COMMAND => $"{concern}{operation}Command",
                OperationType.QUERY => $"{concern}{operation}Query"
            };

            var operationTypeNamespace = ot switch
            {
                OperationType.COMMAND => ResolveNamespace(concern,"Commands",groupBy),
                OperationType.QUERY => ResolveNamespace(concern,"Queries",groupBy)
            };
            
            ClassAssembler
                .Configure(concern, operation, PatternDirectoryType.Handlers, groupBy)
                .ImportNamespaces(new List<NamespaceModel>
                {
                    new NamespaceModel("MediatR"),
                    new NamespaceModel(operationTypeNamespace,true),
                    new NamespaceModel($"Responses.{concern}",true),
                    new NamespaceModel("System.Collections.Generic"),
                    new NamespaceModel("System.Threading"),
                    new NamespaceModel("System.Threading.Tasks")
                })
                .CreateNamespace()
                .CreateClass()
                .WithInheritance(new List<string>
                {
                    $"IRequestHandler<{tInObjectName},{concern}{operation}Response>"
                })
                .ImplementMediatorHandlerInheritance($"{concern}{operation}Response",tInObjectName)
                .Generate()
                ;
        }

        private static string ResolveNamespace(string concern, string operationType, GroupByType groupByType)
            =>  groupByType switch
                    {
                        GroupByType.Concern => $"{concern}.{operationType}",
                        GroupByType.Operation => $"{operationType}.{concern}" 
                    };
    }
}
