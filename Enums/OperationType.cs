using System.ComponentModel;

namespace CQRSAndMediator.Scaffolding.Enums
{
    public enum OperationType
    {
        [Description("COMMAND")]
        COMMAND,
        [Description("QUERY")]
        QUERY,
        [Description("UNSUPPORTED")]
        UNSUPPORTED
    }
}
