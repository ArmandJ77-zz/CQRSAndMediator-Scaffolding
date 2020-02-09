using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildResponse
    {
        public static void Build(string concern, string operation, GroupByType groupBy)
        {
            ClassAssembler
                .Configure(concern, operation, PatternDirectoryType.Responses, groupBy)
                .ImportNamespaces()
                .CreateNamespace()
                .CreateClass()
                .Generate()
                ;
        }
    }
}
