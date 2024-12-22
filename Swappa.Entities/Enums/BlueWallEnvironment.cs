using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum BlueWallEnvironment
    {
        [Description("Development")]
        Dev,
        [Description("DevContainer")]
        Docker,
        [Description("Staging")]
        Stage,
        [Description("Production")]
        Prod
    }
}
