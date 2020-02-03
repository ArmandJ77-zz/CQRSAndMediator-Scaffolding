using CQRSAndMediator.Scaffolding.Enums;
using System;

namespace CQRSAndMediator.Scaffolding.Resolver
{
    public static class PatternFileNameResolver
    {
        public static PatternFileType Resolve(PatternDirectoryType dirType) =>
            dirType switch
            {
                PatternDirectoryType.Commands => PatternFileType.Command,
                PatternDirectoryType.Handlers => PatternFileType.Handler,
                PatternDirectoryType.Queries => PatternFileType.Query,
                PatternDirectoryType.Responses => PatternFileType.Response,
                _ => throw new NotSupportedException()
            };
    }
}
