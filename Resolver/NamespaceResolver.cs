using CQRSAndMediator.Scaffolding.Enums;

namespace CQRSAndMediator.Scaffolding.Resolver
{
    public static class NamespaceResolver
    {
        public static string Resolve(string concern, string operationType, GroupByType groupByType)
            =>  groupByType switch
            {
                GroupByType.Concern => $"{concern}.{operationType}",
                GroupByType.Operation => $"{operationType}.{concern}" 
            };
    }
}
