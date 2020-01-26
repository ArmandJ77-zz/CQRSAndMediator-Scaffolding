using CQRSAndMediator.Scaffolding.Enums;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildOperationType
    {
        public static OperationType Build(string operationType)
        {
            return operationType switch
            {
                "command" => OperationType.COMMAND,
                "query" => OperationType.QUERY,
                _ => OperationType.UNSUPPORTED
            };
        }
    }
}
