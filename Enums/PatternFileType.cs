using System.ComponentModel;

namespace CQRSAndMediator.Scaffolding.Enums
{
    public enum PatternFileType
    {
        [Description("Command")]
        Command,
        [Description("Query")]
        Query,
        [Description("Response")]
        Response,
        [Description("Handler")]
        Handler
    }
}
