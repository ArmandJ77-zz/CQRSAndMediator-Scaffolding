using CQRSAndMediator.Scaffolding.Enums;
using CQRSAndMediator.Scaffolding.Infrastructure;

namespace CQRSAndMediator.Scaffolding.Builders
{
    public static class BuildResponse
    {
        public static void Build(string concern, string operation)
        {
            ClassAssembler
                .Configure(concern,operation,PatternDirectoryType.Responses)
                .ImportNamespaces()
                .CreateNamespace()
                .CreateClass()
                .Generate()
                ;
        }
    }
}
