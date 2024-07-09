using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum Transmission : byte
    {
        [Description("None Specified")]
        None,
        [Description("Manual")]
        Manual,
        [Description("Automatic")]
        Automatic = 1 << 2,
        [Description("Continuously Variable")]
        ContinuouslyVariable = 1 << 3,
        [Description("Semi-Automatic")]
        SemiAutomatic = 1 << 4,
        [Description("Dual Clutch")]
        DualClutch = 1 << 5
    }
}
