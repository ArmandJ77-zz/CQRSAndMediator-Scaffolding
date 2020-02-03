using CQRSAndMediator.Scaffolding.Enums;

namespace CQRSAndMediator.Scaffolding.Resolver
{
    public static class OperationTypeResolver
    {
        public static OperationType Resolve(string operationType) =>
            operationType switch
            {
                "command" => OperationType.COMMAND,
                "query" => OperationType.QUERY,
                _ => OperationType.UNSUPPORTED
            };
    }
}
