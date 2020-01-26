using System.ComponentModel;

namespace CQRSAndMediator.Scaffolding.Enums
{
    public enum PatternDirectoryType
    {
        [Description("Commands")]
        Commands,
        [Description("Queries")]
        Queries,
        [Description("Responses")]
        Responses,
        [Description("Handlers")]
        Handlers
    }
}
