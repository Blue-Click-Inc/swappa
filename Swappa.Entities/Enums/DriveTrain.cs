using System.ComponentModel;

namespace Swappa.Entities.Enums
{
    public enum DriveTrain : byte
    {
        [Description("None Specified")]
        None,
        [Description("RWD")]
        RWD,
        [Description("FWD")]
        FWD = 1 << 1,
        [Description("AWD")]
        AWD = 1 << 2,
        [Description("2WD")]
        TwoWD = 1 << 3,
        [Description("4WD")]
        FourWD = 1 << 4
    }
}
